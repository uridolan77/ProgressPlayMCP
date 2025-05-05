using Microsoft.Extensions.Logging;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.MCP;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace ProgressPlayMCP.Infrastructure.Services;

/// <summary>
/// Implementation of the MCP service
/// </summary>
public class McpService : IMcpService
{
    private readonly IModelConnector _modelConnector;
    private readonly ILogger<McpService> _logger;
    private readonly ConcurrentDictionary<string, McpContextMessage> _contextStore;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="modelConnector">Model connector for AI interactions</param>
    /// <param name="logger">Logger</param>
    public McpService(IModelConnector modelConnector, ILogger<McpService> logger)
    {
        _modelConnector = modelConnector ?? throw new ArgumentNullException(nameof(modelConnector));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contextStore = new ConcurrentDictionary<string, McpContextMessage>();
    }

    /// <summary>
    /// Store a context for future use
    /// </summary>
    /// <param name="contextMessage">The context message to store</param>
    /// <returns>The stored context ID</returns>
    public Task<string> StoreContextAsync(McpContextMessage contextMessage)
    {
        if (contextMessage == null)
            throw new ArgumentNullException(nameof(contextMessage));

        _logger.LogInformation("Storing context with ID {ContextId}", contextMessage.ContextId);
        
        // Ensure context has an ID
        if (string.IsNullOrEmpty(contextMessage.ContextId))
            contextMessage.ContextId = Guid.NewGuid().ToString();

        // Store in dictionary
        _contextStore[contextMessage.ContextId] = contextMessage;
        
        return Task.FromResult(contextMessage.ContextId);
    }

    /// <summary>
    /// Retrieve a stored context
    /// </summary>
    /// <param name="contextId">The context ID to retrieve</param>
    /// <returns>The context message if found, null otherwise</returns>
    public Task<McpContextMessage?> GetContextAsync(string contextId)
    {
        if (string.IsNullOrEmpty(contextId))
            throw new ArgumentException("Context ID cannot be null or empty", nameof(contextId));

        _logger.LogInformation("Retrieving context with ID {ContextId}", contextId);
        
        _contextStore.TryGetValue(contextId, out var contextMessage);
        return Task.FromResult(contextMessage);
    }

    /// <summary>
    /// Process a model request
    /// </summary>
    /// <param name="requestMessage">The request message to process</param>
    /// <returns>A response message from the model</returns>
    public async Task<McpResponseMessage> ProcessRequestAsync(McpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        _logger.LogInformation("Processing request with ID {RequestId}", requestMessage.RequestId);
        
        try
        {
            // Extract model ID from parameters
            if (!requestMessage.Parameters.TryGetValue("model", out var modelIdObj) || modelIdObj is not string modelId)
            {
                throw new ArgumentException("Model ID must be specified in the parameters");
            }
            
            // Verify model availability
            bool isModelAvailable = await _modelConnector.IsModelAvailableAsync(modelId);
            if (!isModelAvailable)
            {
                return CreateErrorResponse(requestMessage.RequestId, "model_unavailable", $"Model '{modelId}' is not available");
            }

            // Gather contexts if provided
            var mergedContext = new Dictionary<string, object>();
            if (requestMessage.Contexts != null && requestMessage.Contexts.Count > 0)
            {
                foreach (var contextId in requestMessage.Contexts)
                {
                    var contextMessage = await GetContextAsync(contextId);
                    if (contextMessage != null)
                    {
                        foreach (var item in contextMessage.Data)
                        {
                            mergedContext[item.Key] = item.Value;
                        }
                    }
                }
            }

            // Merge context with inputs
            var inputs = new Dictionary<string, object>(requestMessage.Inputs);
            if (mergedContext.Count > 0)
            {
                inputs["context"] = mergedContext;
            }

            // Invoke the model
            var outputs = await _modelConnector.InvokeModelAsync(modelId, inputs, requestMessage.Parameters);
            
            // Create response
            return new McpResponseMessage
            {
                RequestId = requestMessage.RequestId,
                Outputs = outputs
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request: {Message}", ex.Message);
            return CreateErrorResponse(requestMessage.RequestId, "processing_error", ex.Message);
        }
    }

    /// <summary>
    /// Process a streaming model request
    /// </summary>
    /// <param name="requestMessage">The request message to process</param>
    /// <returns>A stream of response message chunks from the model</returns>
    public async IAsyncEnumerable<McpResponseMessage> ProcessStreamingRequestAsync(McpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        _logger.LogInformation("Processing streaming request with ID {RequestId}", requestMessage.RequestId);
        
        // Extract model ID from parameters
        if (!requestMessage.Parameters.TryGetValue("model", out var modelIdObj) || modelIdObj is not string modelId)
        {
            yield return CreateErrorResponse(requestMessage.RequestId, "invalid_parameters", "Model ID must be specified in the parameters");
            yield break;
        }
        
        // Verify model availability
        bool isModelAvailable = false;
        McpResponseMessage errorResponse = null;
        
        try
        {
            isModelAvailable = await _modelConnector.IsModelAvailableAsync(modelId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking model availability: {Message}", ex.Message);
            errorResponse = CreateErrorResponse(requestMessage.RequestId, "processing_error", ex.Message);
        }

        if (errorResponse != null)
        {
            yield return errorResponse;
            yield break;
        }

        if (!isModelAvailable)
        {
            yield return CreateErrorResponse(requestMessage.RequestId, "model_unavailable", $"Model '{modelId}' is not available");
            yield break;
        }

        // Gather contexts if provided
        var mergedContext = new Dictionary<string, object>();
        try
        {
            if (requestMessage.Contexts != null && requestMessage.Contexts.Count > 0)
            {
                foreach (var contextId in requestMessage.Contexts)
                {
                    var contextMessage = await GetContextAsync(contextId);
                    if (contextMessage != null)
                    {
                        foreach (var item in contextMessage.Data)
                        {
                            mergedContext[item.Key] = item.Value;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error gathering contexts: {Message}", ex.Message);
            errorResponse = CreateErrorResponse(requestMessage.RequestId, "context_error", $"Error gathering contexts: {ex.Message}");
        }

        if (errorResponse != null)
        {
            yield return errorResponse;
            yield break;
        }

        // Merge context with inputs
        var inputs = new Dictionary<string, object>(requestMessage.Inputs);
        if (mergedContext.Count > 0)
        {
            inputs["context"] = mergedContext;
        }

        // Invoke the model with streaming
        IAsyncEnumerable<Dictionary<string, object>> outputStream = null;
        try
        {
            outputStream = _modelConnector.InvokeModelStreamingAsync(modelId, inputs, requestMessage.Parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing streaming request: {Message}", ex.Message);
            errorResponse = CreateErrorResponse(requestMessage.RequestId, "processing_error", ex.Message);
        }
        
        if (errorResponse != null)
        {
            yield return errorResponse;
            yield break;
        }
        
        // Process the stream
        if (outputStream != null)
        {
            await foreach (var outputChunk in outputStream)
            {
                McpResponseMessage responseMessage = null;
                Exception streamException = null;
                
                try
                {
                    responseMessage = new McpResponseMessage
                    {
                        RequestId = requestMessage.RequestId,
                        Outputs = outputChunk
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during streaming response: {Message}", ex.Message);
                    streamException = ex;
                }
                
                if (streamException != null)
                {
                    yield return CreateErrorResponse(requestMessage.RequestId, "processing_error", streamException.Message);
                    yield break;
                }
                
                if (responseMessage != null)
                {
                    yield return responseMessage;
                }
            }
        }
    }

    /// <summary>
    /// Create an error response
    /// </summary>
    /// <param name="requestId">The request ID</param>
    /// <param name="errorType">Type of error</param>
    /// <param name="errorMessage">Error message</param>
    /// <returns>Error response</returns>
    private static McpResponseMessage CreateErrorResponse(string requestId, string errorType, string errorMessage)
    {
        return new McpResponseMessage
        {
            RequestId = requestId,
            Outputs = new Dictionary<string, object>(),
            Error = new McpError
            {
                Type = errorType,
                Message = errorMessage
            }
        };
    }
}