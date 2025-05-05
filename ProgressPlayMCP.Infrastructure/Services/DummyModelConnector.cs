using Microsoft.Extensions.Logging;
using ProgressPlayMCP.Core.Interfaces;
using System.Runtime.CompilerServices;

namespace ProgressPlayMCP.Infrastructure.Services;

/// <summary>
/// A dummy implementation of the model connector interface for demonstration purposes
/// </summary>
public class DummyModelConnector : IModelConnector
{
    private readonly ILogger<DummyModelConnector> _logger;
    private readonly List<ModelInfo> _availableModels;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public DummyModelConnector(ILogger<DummyModelConnector> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        // Initialize with some sample models
        _availableModels = new List<ModelInfo>
        {
            new ModelInfo
            {
                Id = "gpt-3.5-turbo",
                Name = "GPT-3.5 Turbo",
                Version = "0001",
                Provider = "OpenAI",
                SupportsStreaming = true,
                SupportedInputFormats = new List<string> { "text" },
                SupportedOutputFormats = new List<string> { "text" },
                Capabilities = new List<string> { "chat", "summarization", "translation" }
            },
            new ModelInfo
            {
                Id = "llama-2-70b",
                Name = "Llama 2 70B",
                Version = "0001",
                Provider = "Meta",
                SupportsStreaming = true,
                SupportedInputFormats = new List<string> { "text" },
                SupportedOutputFormats = new List<string> { "text" },
                Capabilities = new List<string> { "chat", "completion", "reasoning" }
            }
        };
    }

    /// <summary>
    /// Get a list of available models
    /// </summary>
    /// <returns>List of available model information</returns>
    public Task<List<ModelInfo>> GetAvailableModelsAsync()
    {
        _logger.LogInformation("Getting available models");
        return Task.FromResult(_availableModels);
    }

    /// <summary>
    /// Check if a specific model is available
    /// </summary>
    /// <param name="modelId">The model identifier</param>
    /// <returns>True if the model is available, false otherwise</returns>
    public Task<bool> IsModelAvailableAsync(string modelId)
    {
        _logger.LogInformation("Checking if model {ModelId} is available", modelId);
        var isAvailable = _availableModels.Any(m => m.Id.Equals(modelId, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(isAvailable);
    }

    /// <summary>
    /// Send a request to a model and get a response
    /// </summary>
    /// <param name="modelId">The model identifier</param>
    /// <param name="inputs">The model inputs</param>
    /// <param name="parameters">Optional model parameters</param>
    /// <returns>The model outputs</returns>
    public Task<Dictionary<string, object>> InvokeModelAsync(string modelId, Dictionary<string, object> inputs, Dictionary<string, object>? parameters = null)
    {
        _logger.LogInformation("Invoking model {ModelId}", modelId);
        
        // For demonstration, we'll just echo the inputs with some additions
        var outputs = new Dictionary<string, object>();
        
        // Check if we have a text input
        if (inputs.TryGetValue("text", out var textObj) && textObj is string text)
        {
            // Simple simulated response
            outputs["text"] = $"This is a simulated response from model {modelId}. You said: {text}";
        }
        // Check if we have a messages input (for chat)
        else if (inputs.TryGetValue("messages", out var messagesObj) && messagesObj is List<object> messages)
        {
            // Simulate a chat response
            outputs["message"] = new Dictionary<string, object>
            {
                ["role"] = "assistant",
                ["content"] = $"This is a simulated chat response from model {modelId}. I received {messages.Count} messages."
            };
        }
        else
        {
            // Default response
            outputs["text"] = $"Simulated response from {modelId}";
        }
        
        // Add some metadata
        outputs["model"] = modelId;
        outputs["timestamp"] = DateTime.UtcNow.ToString("o");
        
        return Task.FromResult(outputs);
    }

    /// <summary>
    /// Send a streaming request to a model and get a stream of responses
    /// </summary>
    /// <param name="modelId">The model identifier</param>
    /// <param name="inputs">The model inputs</param>
    /// <param name="parameters">Optional model parameters</param>
    /// <returns>A stream of model output chunks</returns>
    public async IAsyncEnumerable<Dictionary<string, object>> InvokeModelStreamingAsync(
        string modelId, 
        Dictionary<string, object> inputs, 
        Dictionary<string, object>? parameters = null)
    {
        _logger.LogInformation("Invoking model {ModelId} with streaming", modelId);
        
        // Extract text input if available
        string inputText = "";
        if (inputs.TryGetValue("text", out var textObj) && textObj is string text)
        {
            inputText = text;
        }
        
        // Simulate a streaming response with delays
        var response = $"This is a simulated streaming response from model {modelId}. You said: {inputText}";
        var words = response.Split(' ');
        
        for (int i = 0; i < words.Length; i++)
        {
            // Create a chunk of words
            var chunk = string.Join(" ", words.Skip(i).Take(3));
            i += 2;  // Advance by chunk size minus 1 (the loop increments by 1)
            
            // Return this chunk
            yield return new Dictionary<string, object>
            {
                ["text"] = chunk,
                ["model"] = modelId,
                ["index"] = i / 3,
                ["is_last"] = i >= words.Length - 3
            };
            
            // Simulate processing time
            await Task.Delay(300);
        }
    }
}