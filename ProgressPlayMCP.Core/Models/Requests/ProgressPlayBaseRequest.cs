using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Requests;

/// <summary>
/// Base request model for all ProgressPlay API requests
/// </summary>
public abstract class ProgressPlayBaseRequest
{
    /// <summary>
    /// Array of white label IDs
    /// </summary>
    [JsonPropertyName("WhiteLabels")]
    public List<int> WhiteLabels { get; set; } = new List<int>();
    
    /// <summary>
    /// Affiliate ID for filtering results
    /// </summary>
    [JsonPropertyName("AffiliateId")]
    public string? AffiliateId { get; set; }
}