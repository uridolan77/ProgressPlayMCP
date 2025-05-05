using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.MCP;

/// <summary>
/// Base MCP message
/// </summary>
public class McpMessage
{
    /// <summary>
    /// Message type identifier
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Message version
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = "1.0";
}

/// <summary>
/// MCP context message
/// </summary>
public class McpContextMessage : McpMessage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public McpContextMessage()
    {
        Type = "context";
    }

    /// <summary>
    /// Unique identifier for this context
    /// </summary>
    [JsonPropertyName("context_id")]
    public string ContextId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Context data
    /// </summary>
    [JsonPropertyName("data")]
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
}

/// <summary>
/// MCP request message for model invocation
/// </summary>
public class McpRequestMessage : McpMessage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public McpRequestMessage()
    {
        Type = "request";
    }

    /// <summary>
    /// Unique identifier for this request
    /// </summary>
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Context ID references
    /// </summary>
    [JsonPropertyName("contexts")]
    public List<string> Contexts { get; set; } = new List<string>();

    /// <summary>
    /// Model inputs
    /// </summary>
    [JsonPropertyName("inputs")]
    public Dictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Model parameters
    /// </summary>
    [JsonPropertyName("parameters")]
    public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
}

/// <summary>
/// MCP response message from model invocation
/// </summary>
public class McpResponseMessage : McpMessage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public McpResponseMessage()
    {
        Type = "response";
    }

    /// <summary>
    /// ID of the request this response corresponds to
    /// </summary>
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Model outputs
    /// </summary>
    [JsonPropertyName("outputs")]
    public Dictionary<string, object> Outputs { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Optional error information
    /// </summary>
    [JsonPropertyName("error")]
    public McpError? Error { get; set; }
}

/// <summary>
/// MCP error information
/// </summary>
public class McpError
{
    /// <summary>
    /// Error type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Error message
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional additional error details
    /// </summary>
    [JsonPropertyName("details")]
    public Dictionary<string, object>? Details { get; set; }
}