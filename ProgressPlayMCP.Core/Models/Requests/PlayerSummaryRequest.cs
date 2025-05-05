using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Requests;

/// <summary>
/// Request model for PlayerSummary endpoint
/// </summary>
public class PlayerSummaryRequest : DateRangeRequest
{
    /// <summary>
    /// Currency of the result data
    /// </summary>
    [JsonPropertyName("TargetCurrency")]
    public string? TargetCurrency { get; set; }
}