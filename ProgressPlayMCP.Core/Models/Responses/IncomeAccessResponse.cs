using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for IncomeAccess endpoint
/// </summary>
public class IncomeAccessResponse
{
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    [JsonPropertyName("WhitelabelId")]
    public int? WhitelabelId { get; set; }

    [JsonPropertyName("Tracker")]
    public string Tracker { get; set; } = string.Empty;

    [JsonPropertyName("Dynamic")]
    public string Dynamic { get; set; } = string.Empty;

    [JsonPropertyName("Country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("City")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("Zipcode")]
    public string Zipcode { get; set; } = string.Empty;

    [JsonPropertyName("Currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("Gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("Birthday")]
    public DateTime? Birthday { get; set; }

    [JsonPropertyName("Status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("Platform")]
    public string Platform { get; set; } = string.Empty;

    [JsonPropertyName("RegistrationDate")]
    public DateTime? RegistrationDate { get; set; }

    [JsonPropertyName("FtdDate")]
    public DateTime? FtdDate { get; set; }

    [JsonPropertyName("LastDate")]
    public DateTime? LastDate { get; set; }

    [JsonPropertyName("Registrations")]
    public int? Registrations { get; set; }

    [JsonPropertyName("Ftd")]
    public int? Ftd { get; set; }

    [JsonPropertyName("Deposit")]
    public decimal? Deposit { get; set; }

    [JsonPropertyName("TotalDeposit")]
    public decimal? TotalDeposit { get; set; }

    [JsonPropertyName("Cashout")]
    public decimal? Cashout { get; set; }

    [JsonPropertyName("TotalCashout")]
    public decimal? TotalCashout { get; set; }

    [JsonPropertyName("Bonuses")]
    public decimal? Bonuses { get; set; }

    [JsonPropertyName("ExpiredBonuses")]
    public decimal? ExpiredBonuses { get; set; }

    [JsonPropertyName("Revenue")]
    public decimal? Revenue { get; set; }

    [JsonPropertyName("ClickId")]
    public string? ClickId { get; set; }

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

    [JsonPropertyName("JackpotContribution")]
    public decimal? JackpotContribution { get; set; }
}