using Microsoft.AspNetCore.SignalR;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.MCP;
using System.Text.Json;

namespace ProgressPlayMCP.API.Hubs;

/// <summary>
/// SignalR hub for MCP real-time communication
/// </summary>
public class McpHub : Hub
{
    private readonly IMcpService _mcpService;
    private readonly ILogger<McpHub> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mcpService">MCP service</param>
    /// <param name="logger">Logger</param>
    public McpHub(IMcpService mcpService, ILogger<McpHub> logger)
    {
        _mcpService = mcpService ?? throw new ArgumentNullException(nameof(mcpService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Store a context
    /// </summary>
    /// <param name="contextMessage">Context message to store</param>
    /// <returns>The stored context ID</returns>
    public async Task<string> StoreContext(McpContextMessage contextMessage)
    {
        _logger.LogInformation("WebSocket: Context store request received");
        
        try
        {
            if (contextMessage == null)
            {
                throw new ArgumentNullException(nameof(contextMessage));
            }

            return await _mcpService.StoreContextAsync(contextMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocket: Error storing context: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Process a model request
    /// </summary>
    /// <param name="requestMessage">Request to process</param>
    /// <returns>The response from the model</returns>
    public async Task<McpResponseMessage> ProcessRequest(McpRequestMessage requestMessage)
    {
        _logger.LogInformation("WebSocket: Model request received");
        
        try
        {
            if (requestMessage == null)
            {
                throw new ArgumentNullException(nameof(requestMessage));
            }

            return await _mcpService.ProcessRequestAsync(requestMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocket: Error processing request: {Message}", ex.Message);
            
            return new McpResponseMessage
            {
                RequestId = requestMessage?.RequestId ?? string.Empty,
                Error = new McpError { Type = "server_error", Message = ex.Message }
            };
        }
    }

    /// <summary>
    /// Process a streaming model request
    /// </summary>
    /// <param name="requestMessage">Request to process</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task ProcessStreamingRequest(McpRequestMessage requestMessage)
    {
        _logger.LogInformation("WebSocket: Streaming model request received");
        
        try
        {
            if (requestMessage == null)
            {
                await Clients.Caller.SendAsync("Error", new McpError 
                { 
                    Type = "invalid_request", 
                    Message = "Request message is required" 
                });
                return;
            }

            await foreach (var responseChunk in _mcpService.ProcessStreamingRequestAsync(requestMessage))
            {
                await Clients.Caller.SendAsync("StreamResponse", responseChunk);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocket: Error processing streaming request: {Message}", ex.Message);
            
            await Clients.Caller.SendAsync("Error", new McpError
            {
                Type = "server_error",
                Message = ex.Message
            });
        }
    }
}