using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.MCP;
using System.Text.Json;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for MCP (Model Context Protocol) operations
/// </summary>
[ApiController]
[Route("mcp")]
public class McpController : ControllerBase
{
    private readonly IMcpService _mcpService;
    private readonly ILogger<McpController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mcpService">MCP service</param>
    /// <param name="logger">Logger</param>
    public McpController(IMcpService mcpService, ILogger<McpController> logger)
    {
        _mcpService = mcpService ?? throw new ArgumentNullException(nameof(mcpService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Store a context for future use
    /// </summary>
    /// <param name="contextMessage">The context message to store</param>
    /// <returns>The stored context ID</returns>
    [HttpPost("context")]
    [ProducesResponseType(typeof(McpContextMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> StoreContext(McpContextMessage contextMessage)
    {
        try
        {
            _logger.LogInformation("Context store request received");
            
            if (contextMessage == null)
            {
                return BadRequest("Context message is required");
            }

            // Validate message type
            if (contextMessage.Type != "context")
            {
                return BadRequest("Invalid message type. Expected 'context'");
            }

            await _mcpService.StoreContextAsync(contextMessage);
            return Ok(contextMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing context: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new McpError { Type = "server_error", Message = "Error storing context" });
        }
    }

    /// <summary>
    /// Retrieve a stored context
    /// </summary>
    /// <param name="contextId">The context ID to retrieve</param>
    /// <returns>The context message if found</returns>
    [HttpGet("context/{contextId}")]
    [ProducesResponseType(typeof(McpContextMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetContext(string contextId)
    {
        try
        {
            _logger.LogInformation("Context retrieval request received for ID {ContextId}", contextId);
            
            var context = await _mcpService.GetContextAsync(contextId);
            if (context == null)
            {
                return NotFound(new McpError { Type = "not_found", Message = $"Context with ID '{contextId}' not found" });
            }

            return Ok(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving context: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new McpError { Type = "server_error", Message = "Error retrieving context" });
        }
    }

    /// <summary>
    /// Process a model request
    /// </summary>
    /// <param name="requestMessage">The request message to process</param>
    /// <returns>A response message from the model</returns>
    [HttpPost("request")]
    [ProducesResponseType(typeof(McpResponseMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProcessRequest(McpRequestMessage requestMessage)
    {
        try
        {
            _logger.LogInformation("Model request received");
            
            if (requestMessage == null)
            {
                return BadRequest("Request message is required");
            }

            // Validate message type
            if (requestMessage.Type != "request")
            {
                return BadRequest("Invalid message type. Expected 'request'");
            }

            var response = await _mcpService.ProcessRequestAsync(requestMessage);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new McpResponseMessage 
                { 
                    RequestId = requestMessage?.RequestId ?? string.Empty,
                    Error = new McpError { Type = "server_error", Message = ex.Message }
                });
        }
    }

    /// <summary>
    /// Process a streaming model request
    /// </summary>
    /// <param name="requestMessage">The request message to process</param>
    /// <returns>A stream of response message chunks from the model</returns>
    [HttpPost("stream")]
    [ProducesResponseType(typeof(IAsyncEnumerable<McpResponseMessage>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task StreamRequest(McpRequestMessage requestMessage)
    {
        try
        {
            _logger.LogInformation("Streaming model request received");
            
            if (requestMessage == null)
            {
                await WriteErrorResponse(new McpError { Type = "invalid_request", Message = "Request message is required" });
                return;
            }

            // Validate message type
            if (requestMessage.Type != "request")
            {
                await WriteErrorResponse(new McpError { Type = "invalid_request", Message = "Invalid message type. Expected 'request'" });
                return;
            }

            Response.ContentType = "text/event-stream";
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            await foreach (var responseChunk in _mcpService.ProcessStreamingRequestAsync(requestMessage))
            {
                await WriteResponseChunk(responseChunk);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing streaming request: {Message}", ex.Message);
            await WriteErrorResponse(new McpError
            {
                Type = "server_error",
                Message = ex.Message
            });
        }
    }

    private async Task WriteResponseChunk(McpResponseMessage response)
    {
        var json = JsonSerializer.Serialize(response);
        await Response.WriteAsync($"data: {json}\n\n");
        await Response.Body.FlushAsync();
    }

    private async Task WriteErrorResponse(McpError error)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = StatusCodes.Status400BadRequest;
        
        var errorResponse = new McpResponseMessage
        {
            Error = error
        };
        
        var json = JsonSerializer.Serialize(errorResponse);
        await Response.WriteAsync(json);
    }
}