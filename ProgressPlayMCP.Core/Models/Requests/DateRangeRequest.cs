using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Requests;

/// <summary>
/// Base request model for endpoints that require a date range
/// </summary>
public abstract class DateRangeRequest : ProgressPlayBaseRequest
{
    /// <summary>
    /// Start date of the period
    /// </summary>
    [JsonPropertyName("DateStart")]
    public string DateStart { get; set; } = string.Empty;

    /// <summary>
    /// End date of the period
    /// </summary>
    [JsonPropertyName("DateEnd")]
    public string DateEnd { get; set; } = string.Empty;
}