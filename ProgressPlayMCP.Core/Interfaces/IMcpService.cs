using ProgressPlayMCP.Core.Models.MCP;

namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Service for handling MCP operations
/// </summary>
public interface IMcpService
{
    /// <summary>
    /// Store a context for future use
    /// </summary>
    /// <param name="contextMessage">The context message to store</param>
    /// <returns>The stored context ID</returns>
    Task<string> StoreContextAsync(McpContextMessage contextMessage);
    
    /// <summary>
    /// Retrieve a stored context
    /// </summary>
    /// <param name="contextId">The context ID to retrieve</param>
    /// <returns>The context message if found, null otherwise</returns>
    Task<McpContextMessage?> GetContextAsync(string contextId);
    
    /// <summary>
    /// Process a model request
    /// </summary>
    /// <param name="requestMessage">The request message to process</param>
    /// <returns>A response message from the model</returns>
    Task<McpResponseMessage> ProcessRequestAsync(McpRequestMessage requestMessage);

    /// <summary>
    /// Process a streaming model request
    /// </summary>
    /// <param name="requestMessage">The request message to process</param>
    /// <returns>A stream of response message chunks from the model</returns>
    IAsyncEnumerable<McpResponseMessage> ProcessStreamingRequestAsync(McpRequestMessage requestMessage);
}