using ProgressPlayMCP.Core.Models.MCP;

namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Interface for connecting to external AI models
/// </summary>
public interface IModelConnector
{
    /// <summary>
    /// Get a list of available models
    /// </summary>
    /// <returns>List of available model information</returns>
    Task<List<ModelInfo>> GetAvailableModelsAsync();
    
    /// <summary>
    /// Check if a specific model is available
    /// </summary>
    /// <param name="modelId">The model identifier</param>
    /// <returns>True if the model is available, false otherwise</returns>
    Task<bool> IsModelAvailableAsync(string modelId);
    
    /// <summary>
    /// Send a request to a model and get a response
    /// </summary>
    /// <param name="modelId">The model identifier</param>
    /// <param name="inputs">The model inputs</param>
    /// <param name="parameters">Optional model parameters</param>
    /// <returns>The model outputs</returns>
    Task<Dictionary<string, object>> InvokeModelAsync(string modelId, Dictionary<string, object> inputs, Dictionary<string, object>? parameters = null);
    
    /// <summary>
    /// Send a streaming request to a model and get a stream of responses
    /// </summary>
    /// <param name="modelId">The model identifier</param>
    /// <param name="inputs">The model inputs</param>
    /// <param name="parameters">Optional model parameters</param>
    /// <returns>A stream of model output chunks</returns>
    IAsyncEnumerable<Dictionary<string, object>> InvokeModelStreamingAsync(string modelId, Dictionary<string, object> inputs, Dictionary<string, object>? parameters = null);
}

/// <summary>
/// Information about an AI model
/// </summary>
public class ModelInfo
{
    /// <summary>
    /// Model identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Human-readable model name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Model version
    /// </summary>
    public string Version { get; set; } = string.Empty;
    
    /// <summary>
    /// Model provider
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the model supports streaming
    /// </summary>
    public bool SupportsStreaming { get; set; }
    
    /// <summary>
    /// Supported input formats
    /// </summary>
    public List<string> SupportedInputFormats { get; set; } = new List<string>();
    
    /// <summary>
    /// Supported output formats
    /// </summary>
    public List<string> SupportedOutputFormats { get; set; } = new List<string>();
    
    /// <summary>
    /// Model capabilities
    /// </summary>
    public List<string> Capabilities { get; set; } = new List<string>();
}