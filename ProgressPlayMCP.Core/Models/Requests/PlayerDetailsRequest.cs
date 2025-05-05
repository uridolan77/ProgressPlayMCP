using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Requests;

/// <summary>
/// Request model for PlayerDetails endpoint
/// </summary>
public class PlayerDetailsRequest : ProgressPlayBaseRequest
{
    /// <summary>
    /// Get players who registered after the given date
    /// </summary>
    [JsonPropertyName("RegistrationDateStart")]
    public string? RegistrationDateStart { get; set; }

    /// <summary>
    /// Get players who registered before the given date
    /// </summary>
    [JsonPropertyName("RegistrationDateEnd")]
    public string? RegistrationDateEnd { get; set; }

    /// <summary>
    /// Get players who was updated or played after the given date
    /// </summary>
    [JsonPropertyName("LastUpdatedDateStart")]
    public string? LastUpdatedDateStart { get; set; }

    /// <summary>
    /// Get players who was updated or played before the given date
    /// </summary>
    [JsonPropertyName("LastUpdatedDateEnd")]
    public string? LastUpdatedDateEnd { get; set; }
}