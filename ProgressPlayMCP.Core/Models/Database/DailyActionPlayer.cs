using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a player in the daily actions database
/// </summary>
public class DailyActionPlayer
{
    /// <summary>
    /// Player ID (primary key)
    /// </summary>
    [Key]
    public long? PlayerID { get; set; }

    /// <summary>
    /// Casino name
    /// </summary>
    public string CasinoName { get; set; }

    /// <summary>
    /// White label ID/Casino ID
    /// </summary>
    public int? CasinoID { get; set; }

    /// <summary>
    /// Player alias/username
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Registration date
    /// </summary>
    public DateTime? RegisteredDate { get; set; }

    /// <summary>
    /// First deposit date
    /// </summary>
    public DateTime? FirstDepositDate { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Gender
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Country code
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Currency symbol
    /// </summary>
    public string CurrencySymbol { get; set; }

    /// <summary>
    /// Current balance
    /// </summary>
    public decimal? Balance { get; set; }

    /// <summary>
    /// Original balance in local currency
    /// </summary>
    public decimal? OriginalBalance { get; set; }

    /// <summary>
    /// Affiliate ID
    /// </summary>
    public string AffiliateID { get; set; }

    /// <summary>
    /// Language
    /// </summary>
    public string Language { get; set; }

    /// <summary>
    /// Platform used for registration
    /// </summary>
    public string RegisteredPlatform { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Whether player has opted in for communications
    /// </summary>
    public bool? IsOptIn { get; set; }

    /// <summary>
    /// Whether player is blocked
    /// </summary>
    public bool? IsBlocked { get; set; }

    /// <summary>
    /// Whether player is a test account
    /// </summary>
    public bool? IsTest { get; set; }

    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// VIP level
    /// </summary>
    public int? VIPLevel { get; set; }

    /// <summary>
    /// Total deposits
    /// </summary>
    public decimal? TotalDeposits { get; set; }

    /// <summary>
    /// Total withdrawals
    /// </summary>
    public decimal? TotalWithdrawals { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Dynamic parameter
    /// </summary>
    public string DynamicParameter { get; set; }

    /// <summary>
    /// Click ID
    /// </summary>
    public string ClickId { get; set; }

    /// <summary>
    /// Documents status
    /// </summary>
    public string DocumentsStatus { get; set; }

    /// <summary>
    /// Platform string
    /// </summary>
    public string PlatformString { get; set; }

    /// <summary>
    /// Whether SMS communications are enabled
    /// </summary>
    public bool? SMSEnabled { get; set; }

    /// <summary>
    /// Whether email communications are enabled
    /// </summary>
    public bool? MailEnabled { get; set; }

    /// <summary>
    /// Whether promotions are enabled
    /// </summary>
    public bool? PromotionsEnabled { get; set; }

    /// <summary>
    /// Whether bonuses are enabled
    /// </summary>
    public bool? BonusesEnabled { get; set; }

    /// <summary>
    /// IP address
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// Whether promotional bingo SMS are enabled
    /// </summary>
    public bool? PromotionalBingoSMSEnabled { get; set; }

    /// <summary>
    /// Whether promotional sports emails are enabled
    /// </summary>
    public bool? PromotionalSportsEmailEnabled { get; set; }

    /// <summary>
    /// Whether promotional sports SMS are enabled
    /// </summary>
    public bool? PromotionalSportsSMSEnabled { get; set; }

    /// <summary>
    /// Whether promotional push notifications are enabled
    /// </summary>
    public bool? PromotionalPushEnabled { get; set; }

    /// <summary>
    /// Whether promotional phone calls are enabled
    /// </summary>
    public bool? PromotionalPhoneEnabled { get; set; }

    /// <summary>
    /// Whether promotional post is enabled
    /// </summary>
    public bool? PromotionalPostEnabled { get; set; }

    /// <summary>
    /// Whether promotional partner communications are enabled
    /// </summary>
    public bool? PromotionalPartnerEnabled { get; set; }

    /// <summary>
    /// Occupation yearly income
    /// </summary>
    public decimal? OccupationYearlyIncome { get; set; }

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime? LastUpdate { get; set; }

    /// <summary>
    /// Navigation property to daily actions
    /// </summary>
    public virtual ICollection<DailyAction> DailyActions { get; set; }

    /// <summary>
    /// Navigation property to daily action games
    /// </summary>
    public virtual ICollection<DailyActionGame> DailyActionGames { get; set; }

    /// <summary>
    /// Navigation property to transactions
    /// </summary>
    public virtual ICollection<DailyActionTransaction> DailyActionTransactions { get; set; }

    /// <summary>
    /// Navigation property to bonus balances
    /// </summary>
    public virtual ICollection<BonusBalance> BonusBalances { get; set; }

    /// <summary>
    /// Navigation property to withdrawal requests
    /// </summary>
    public virtual ICollection<WithdrawalRequest> WithdrawalRequests { get; set; }

    /// <summary>
    /// Navigation property to big winners
    /// </summary>
    public virtual ICollection<BigWinner> BigWinners { get; set; }
}