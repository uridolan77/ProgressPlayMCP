using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Requests;

/// <summary>
/// Request model for IncomeAccess endpoint
/// </summary>
public class IncomeAccessRequest
{
    /// <summary>
    /// Currency of the result data
    /// </summary>
    [JsonPropertyName("TargetCurrency")]
    public string TargetCurrency { get; set; } = string.Empty;

    /// <summary>
    /// ID of the white label
    /// </summary>
    [JsonPropertyName("WhitelabelId")]
    public int WhitelabelId { get; set; }

    /// <summary>
    /// Start date of the period
    /// </summary>
    [JsonPropertyName("StartDate")]
    public string StartDate { get; set; } = string.Empty;

    /// <summary>
    /// End date of the period
    /// </summary>
    [JsonPropertyName("EndDate")]
    public string EndDate { get; set; } = string.Empty;
}