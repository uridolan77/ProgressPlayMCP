using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for PlayerGames endpoint
/// </summary>
public class PlayerGameResponse
{
    [JsonPropertyName("GameDate")]
    public DateTime? GameDate { get; set; }

    [JsonPropertyName("PlayerID")]
    public int? PlayerId { get; set; }

    [JsonPropertyName("WhitelabelId")]
    public int? WhitelabelId { get; set; }

    [JsonPropertyName("CasinoName")]
    public string CasinoName { get; set; } = string.Empty;

    [JsonPropertyName("GameID")]
    public int? GameId { get; set; }

    [JsonPropertyName("GameType")]
    public string GameType { get; set; } = string.Empty;

    [JsonPropertyName("Provider")]
    public string Provider { get; set; } = string.Empty;

    [JsonPropertyName("SubProvider")]
    public string? SubProvider { get; set; }

    [JsonPropertyName("GameName")]
    public string GameName { get; set; } = string.Empty;

    [JsonPropertyName("Currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("CurrencySymbol")]
    public string CurrencySymbol { get; set; } = string.Empty;

    [JsonPropertyName("Bets")]
    public decimal? Bets { get; set; }
    
    /// <summary>
    /// Gets the Bets value or 0 if null
    /// </summary>
    [JsonIgnore]
    public decimal BetsValue => Bets ?? 0m;

    [JsonPropertyName("Wins")]
    public decimal? Wins { get; set; }
    
    /// <summary>
    /// Gets the Wins value or 0 if null
    /// </summary>
    [JsonIgnore]
    public decimal WinsValue => Wins ?? 0m;

    [JsonPropertyName("Ggr")]
    public decimal? Ggr { get; set; }
    
    /// <summary>
    /// Gets the Ggr value or 0 if null
    /// </summary>
    [JsonIgnore]
    public decimal GgrValue => Ggr ?? 0m;

    [JsonPropertyName("TotalRecords")]
    public int? TotalRecords { get; set; }
}