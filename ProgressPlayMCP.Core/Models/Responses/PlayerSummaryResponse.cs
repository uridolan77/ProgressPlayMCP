using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for PlayerSummary endpoint
/// </summary>
public class PlayerSummaryResponse
{
    [JsonPropertyName("PlayerId")]
    public int? PlayerId { get; set; }

    [JsonPropertyName("WhitelabelId")]
    public int? WhitelabelId { get; set; }

    [JsonPropertyName("Alias")]
    public string Alias { get; set; } = string.Empty;

    [JsonPropertyName("CasinoName")]
    public string CasinoName { get; set; } = string.Empty;

    [JsonPropertyName("Tracker")]
    public string Tracker { get; set; } = string.Empty;

    [JsonPropertyName("DynamicParameter")]
    public string DynamicParameter { get; set; } = string.Empty;

    [JsonPropertyName("ClickId")]
    public string? ClickId { get; set; }

    [JsonPropertyName("Deposit")]
    public decimal? Deposit { get; set; }

    [JsonPropertyName("Cashout")]
    public decimal? Cashout { get; set; }

    [JsonPropertyName("Bonuses")]
    public decimal? Bonuses { get; set; }

    [JsonPropertyName("ExpiredBonuses")]
    public decimal? ExpiredBonuses { get; set; }

    [JsonPropertyName("Chargeback")]
    public decimal? Chargeback { get; set; }

    [JsonPropertyName("Void")]
    public decimal? Void { get; set; }

    [JsonPropertyName("ReverseChargeback")]
    public decimal? ReverseChargeback { get; set; }

    [JsonPropertyName("SideGamesBets")]
    public decimal? SideGamesBets { get; set; }

    [JsonPropertyName("SideGamesWins")]
    public decimal? SideGamesWins { get; set; }

    [JsonPropertyName("BetsCasino")]
    public decimal? BetsCasino { get; set; }

    [JsonPropertyName("WinsCasino")]
    public decimal? WinsCasino { get; set; }

    [JsonPropertyName("BetsSport")]
    public decimal? BetsSport { get; set; }

    [JsonPropertyName("WinsSport")]
    public decimal? WinsSport { get; set; }

    [JsonPropertyName("Ggr")]
    public decimal? Ggr { get; set; }

    [JsonPropertyName("JackpotContribution")]
    public decimal? JackpotContribution { get; set; }

    [JsonPropertyName("ClubPointsConversion")]
    public decimal? ClubPointsConversion { get; set; }

    [JsonPropertyName("RevenueAdjustments")]
    public decimal? RevenueAdjustments { get; set; }

    [JsonPropertyName("Revenue")]
    public decimal? Revenue { get; set; }

    [JsonPropertyName("Age")]
    public int? Age { get; set; }

    [JsonPropertyName("Gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("Country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("Currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("Status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("BlockReason")]
    public string BlockReason { get; set; } = string.Empty;

    [JsonPropertyName("TotalRecords")]
    public int? TotalRecords { get; set; }
}