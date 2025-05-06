using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for PlayerDetails endpoint
/// </summary>
public class PlayerDetailsResponse
{
    [JsonPropertyName("PlayerID")]
    public long? PlayerId { get; set; }

    [JsonPropertyName("WhitelabelId")]
    public int? WhitelabelId { get; set; }

    [JsonPropertyName("Alias")]
    public string Alias { get; set; } = string.Empty;

    [JsonPropertyName("Username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("CasinoName")]
    public string CasinoName { get; set; } = string.Empty;

    [JsonPropertyName("Language")]
    public string Language { get; set; } = string.Empty;

    [JsonPropertyName("AffiliateID")]
    public string AffiliateId { get; set; } = string.Empty;

    [JsonPropertyName("DynamicParameter")]
    public string DynamicParameter { get; set; } = string.Empty;

    [JsonPropertyName("ClickID")]
    public string ClickId { get; set; } = string.Empty;

    [JsonPropertyName("PromotionCode")]
    public string? PromotionCode { get; set; }

    [JsonPropertyName("Country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("City")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("Gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("DateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    [JsonPropertyName("IP")]
    public string IP { get; set; } = string.Empty;

    [JsonPropertyName("RegisteredDate")]
    public DateTime? RegisteredDate { get; set; }

    [JsonPropertyName("FirstDepositDate")]
    public DateTime? FirstDepositDate { get; set; }

    [JsonPropertyName("LastLoginDate")]
    public DateTime? LastLoginDate { get; set; }

    [JsonPropertyName("LastDepositDate")]
    public DateTime? LastDepositDate { get; set; }

    [JsonPropertyName("RegisteredPlatform")]
    public string RegisteredPlatform { get; set; } = string.Empty;

    [JsonPropertyName("BonusesEnabled")]
    public bool? BonusesEnabled { get; set; }

    [JsonPropertyName("PromotionsMailEnabled")]
    public bool? PromotionsMailEnabled { get; set; }

    [JsonPropertyName("PromotionsSMSEnabled")]
    public bool? PromotionsSMSEnabled { get; set; }

    [JsonPropertyName("PromotionsPhoneEnabled")]
    public bool? PromotionsPhoneEnabled { get; set; }

    [JsonPropertyName("PromotionsPostEnabled")]
    public bool? PromotionsPostEnabled { get; set; }

    [JsonPropertyName("Status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("IsBlocked")]
    public bool? IsBlocked { get; set; }

    [JsonPropertyName("BlockDate")]
    public DateTime? BlockDate { get; set; }

    [JsonPropertyName("BlockReason")]
    public string BlockReason { get; set; } = string.Empty;

    [JsonPropertyName("BlockReleaseDate")]
    public DateTime? BlockReleaseDate { get; set; }

    [JsonPropertyName("BlockType")]
    public string BlockType { get; set; } = string.Empty;

    [JsonPropertyName("Currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("Balance")]
    public decimal? Balance { get; set; }

    [JsonPropertyName("AccountBalance")]
    public decimal? AccountBalance { get; set; }

    [JsonPropertyName("BonusBalance")]
    public decimal? BonusBalance { get; set; }

    [JsonPropertyName("MaxBalance")]
    public decimal? MaxBalance { get; set; }

    [JsonPropertyName("DepositsCount")]
    public int? DepositsCount { get; set; }

    [JsonPropertyName("TotalDeposits")]
    public decimal? TotalDeposits { get; set; }

    [JsonPropertyName("TotalWithdrawals")]
    public decimal? TotalWithdrawals { get; set; }

    [JsonPropertyName("TotalChargeBacks")]
    public decimal? TotalChargeBacks { get; set; }

    [JsonPropertyName("TotalChargebackReverses")]
    public decimal? TotalChargebackReverses { get; set; }

    [JsonPropertyName("TotalVoids")]
    public decimal? TotalVoids { get; set; }

    [JsonPropertyName("TotalBonuses")]
    public decimal? TotalBonuses { get; set; }

    [JsonPropertyName("TotalCustomerClubPoints")]
    public decimal? TotalCustomerClubPoints { get; set; }

    [JsonPropertyName("RegistrationPlayMode")]
    public string RegistrationPlayMode { get; set; } = string.Empty;

    [JsonPropertyName("TotalBetsCasino")]
    public decimal? TotalBetsCasino { get; set; }

    [JsonPropertyName("TotalBetsSport")]
    public decimal? TotalBetsSport { get; set; }

    [JsonPropertyName("Age")]
    public int? Age { get; set; }

    [JsonPropertyName("CurrencySymbol")]
    public string CurrencySymbol { get; set; } = string.Empty;

    [JsonPropertyName("LastUpdated")]
    public DateTime? LastUpdated { get; set; }

    [JsonPropertyName("TotalRecords")]
    public int? TotalRecords { get; set; }
}