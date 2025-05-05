using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Requests;

/// <summary>
/// Request model for DailyActions endpoint
/// </summary>
public class DailyActionsRequest : DateRangeRequest
{
    /// <summary>
    /// Currency of the result data
    /// </summary>
    [JsonPropertyName("TargetCurrency")]
    public string? TargetCurrency { get; set; }
}