using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for DailyActions endpoint
/// </summary>
public class DailyActionResponse
{
    [JsonPropertyName("Id")]
    public long? Id { get; set; }

    [JsonPropertyName("Date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("WhitelabelId")]
    public int? WhitelabelId { get; set; }

    [JsonPropertyName("Playerid")]
    public int? PlayerId { get; set; }

    [JsonPropertyName("Registration")]
    public int? Registration { get; set; }

    [JsonPropertyName("Ftd")]
    public int? Ftd { get; set; }

    [JsonPropertyName("Ftda")]
    public int? Ftda { get; set; }

    [JsonPropertyName("Deposits")]
    public decimal? Deposits { get; set; }

    [JsonPropertyName("Depositscreditcard")]
    public decimal? DepositsCreditCard { get; set; }

    [JsonPropertyName("Depositsneteller")]
    public decimal? DepositsNeteller { get; set; }

    [JsonPropertyName("Depositsmoneybookers")]
    public decimal? DepositsMoneyBookers { get; set; }

    [JsonPropertyName("Depositsother")]
    public decimal? DepositsOther { get; set; }

    [JsonPropertyName("Cashoutrequests")]
    public decimal? CashoutRequests { get; set; }

    [JsonPropertyName("Paidcashouts")]
    public decimal? PaidCashouts { get; set; }

    [JsonPropertyName("Chargebacks")]
    public decimal? Chargebacks { get; set; }

    [JsonPropertyName("Voids")]
    public decimal? Voids { get; set; }

    [JsonPropertyName("Reversechargebacks")]
    public decimal? ReverseChargebacks { get; set; }

    [JsonPropertyName("Bonuses")]
    public decimal? Bonuses { get; set; }

    [JsonPropertyName("Collectedbonuses")]
    public decimal? CollectedBonuses { get; set; }

    [JsonPropertyName("Expiredbonuses")]
    public decimal? ExpiredBonuses { get; set; }

    [JsonPropertyName("Clubpointsconversion")]
    public decimal? ClubPointsConversion { get; set; }

    [JsonPropertyName("Bankroll")]
    public decimal? Bankroll { get; set; }

    [JsonPropertyName("Sidegamesbets")]
    public decimal? SideGamesBets { get; set; }

    [JsonPropertyName("Sidegamesrefunds")]
    public decimal? SideGamesRefunds { get; set; }

    [JsonPropertyName("Sidegameswins")]
    public decimal? SideGamesWins { get; set; }

    [JsonPropertyName("Sidegamestablegamesbets")]
    public decimal? SideGamesTableGamesBets { get; set; }

    [JsonPropertyName("Sidegamestablegameswins")]
    public decimal? SideGamesTableGamesWins { get; set; }

    [JsonPropertyName("Sidegamescasualgamesbets")]
    public decimal? SideGamesCasualGamesBets { get; set; }

    [JsonPropertyName("Sidegamescasualgameswins")]
    public decimal? SideGamesCasualGamesWins { get; set; }

    [JsonPropertyName("Sidegamesslotsbets")]
    public decimal? SideGamesSlotsBets { get; set; }

    [JsonPropertyName("Sidegamesslotswins")]
    public decimal? SideGamesSlotsWins { get; set; }

    [JsonPropertyName("Sidegamesjackpotsbets")]
    public decimal? SideGamesJackpotsBets { get; set; }

    [JsonPropertyName("Sidegamesjackpotswins")]
    public decimal? SideGamesJackpotsWins { get; set; }

    [JsonPropertyName("Sidegamesfeaturedbets")]
    public decimal? SideGamesFeaturedBets { get; set; }

    [JsonPropertyName("Sidegamesfeaturedwins")]
    public decimal? SideGamesFeaturedWins { get; set; }

    [JsonPropertyName("Jackpotcontribution")]
    public decimal? JackpotContribution { get; set; }

    [JsonPropertyName("Lottobets")]
    public decimal? LottoBets { get; set; }

    [JsonPropertyName("Lottoadvancedbets")]
    public decimal? LottoAdvancedBets { get; set; }

    [JsonPropertyName("Lottowins")]
    public decimal? LottoWins { get; set; }

    [JsonPropertyName("Lottoadvancedwins")]
    public decimal? LottoAdvancedWins { get; set; }

    [JsonPropertyName("Insurancecontribution")]
    public decimal? InsuranceContribution { get; set; }

    [JsonPropertyName("Adjustments")]
    public decimal? Adjustments { get; set; }

    [JsonPropertyName("Adjustmentsadd")]
    public decimal? AdjustmentsAdd { get; set; }

    [JsonPropertyName("Clearedbalance")]
    public decimal? ClearedBalance { get; set; }

    [JsonPropertyName("Revenueadjustments")]
    public decimal? RevenueAdjustments { get; set; }

    [JsonPropertyName("Revenueadjustmentsadd")]
    public decimal? RevenueAdjustmentsAdd { get; set; }

    [JsonPropertyName("Revenue")]
    public decimal? Revenue { get; set; }

    [JsonPropertyName("BetsCasino")]
    public decimal? BetsCasino { get; set; }

    [JsonPropertyName("WinsCasino")]
    public decimal? WinsCasino { get; set; }

    [JsonPropertyName("BetsSport")]
    public decimal? BetsSport { get; set; }

    [JsonPropertyName("WinsSport")]
    public decimal? WinsSport { get; set; }

    [JsonPropertyName("Updateddate")]
    public DateTime? UpdatedDate { get; set; }
}