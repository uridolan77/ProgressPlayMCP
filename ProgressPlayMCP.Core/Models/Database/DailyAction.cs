using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a daily action record
/// </summary>
public class DailyAction
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Key]
    public long? ID { get; set; }

    /// <summary>
    /// Player ID (foreign key)
    /// </summary>
    public long? PlayerID { get; set; }

    /// <summary>
    /// First Time Deposit indicator
    /// </summary>
    public bool? FTD { get; set; }

    /// <summary>
    /// EUR to GBP conversion rate
    /// </summary>
    public decimal? EUR2GBP { get; set; }

    /// <summary>
    /// Action date
    /// </summary>
    public DateTime? ActionDate { get; set; }

    /// <summary>
    /// Total deposits amount for the day
    /// </summary>
    public decimal? TotalDeposits { get; set; }

    /// <summary>
    /// Total withdrawals amount for the day
    /// </summary>
    public decimal? TotalWithdrawals { get; set; }

    /// <summary>
    /// Total wagers amount for the day
    /// </summary>
    public decimal? TotalWagers { get; set; }

    /// <summary>
    /// Total wins amount for the day
    /// </summary>
    public decimal? TotalWins { get; set; }

    /// <summary>
    /// Net gaming revenue for the day
    /// </summary>
    public decimal? NetGamingRevenue { get; set; }

    /// <summary>
    /// Total bonus amount awarded for the day
    /// </summary>
    public decimal? TotalBonusAmount { get; set; }

    /// <summary>
    /// Number of games played during the day
    /// </summary>
    public int? GamesPlayedCount { get; set; }

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Total deposits in original currency
    /// </summary>
    public decimal? TotalDepositsOriginal { get; set; }

    /// <summary>
    /// Total withdrawals in original currency
    /// </summary>
    public decimal? TotalWithdrawalsOriginal { get; set; }

    /// <summary>
    /// Total wagers in original currency
    /// </summary>
    public decimal? TotalWagersOriginal { get; set; }

    /// <summary>
    /// Total wins in original currency
    /// </summary>
    public decimal? TotalWinsOriginal { get; set; }

    /// <summary>
    /// Net gaming revenue in original currency
    /// </summary>
    public decimal? NetGamingRevenueOriginal { get; set; }

    /// <summary>
    /// Total bonus amount in original currency
    /// </summary>
    public decimal? TotalBonusAmountOriginal { get; set; }

    /// <summary>
    /// Current player balance
    /// </summary>
    public decimal? CurrentBalance { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// First Time Deposit Amount indicator
    /// </summary>
    public bool? FTDA { get; set; }

    /// <summary>
    /// Deposits for sport category
    /// </summary>
    public decimal? DepositsSport { get; set; }

    /// <summary>
    /// Deposits for live games category
    /// </summary>
    public decimal? DepositsLive { get; set; }

    /// <summary>
    /// Cashout requests amount
    /// </summary>
    public decimal? CashoutRequests { get; set; }

    /// <summary>
    /// Paid cashouts amount
    /// </summary>
    public decimal? PaidCashouts { get; set; }

    /// <summary>
    /// Chargebacks amount
    /// </summary>
    public decimal? Chargebacks { get; set; }

    /// <summary>
    /// Voided transactions amount
    /// </summary>
    public decimal? Voids { get; set; }

    /// <summary>
    /// Reverse chargebacks amount
    /// </summary>
    public decimal? ReverseChargebacks { get; set; }

    /// <summary>
    /// Bonuses amount
    /// </summary>
    public decimal? Bonuses { get; set; }

    /// <summary>
    /// Sport bonuses amount
    /// </summary>
    public decimal? BonusesSport { get; set; }

    /// <summary>
    /// Collected bonuses amount
    /// </summary>
    public decimal? CollectedBonuses { get; set; }

    /// <summary>
    /// Bingo bets amount
    /// </summary>
    public decimal? BetsBingo { get; set; }

    /// <summary>
    /// Bingo refunds amount
    /// </summary>
    public decimal? RefundsBingo { get; set; }

    /// <summary>
    /// Bingo wins amount
    /// </summary>
    public decimal? WinsBingo { get; set; }

    /// <summary>
    /// Real money bingo bets amount
    /// </summary>
    public decimal? BetsBingoReal { get; set; }

    /// <summary>
    /// Real money bingo refunds amount
    /// </summary>
    public decimal? RefundsBingoReal { get; set; }

    /// <summary>
    /// Real money bingo wins amount
    /// </summary>
    public decimal? WinsBingoReal { get; set; }

    /// <summary>
    /// Bonus bingo bets amount
    /// </summary>
    public decimal? BetsBingoBonus { get; set; }

    /// <summary>
    /// Bonus bingo refunds amount
    /// </summary>
    public decimal? RefundsBingoBonus { get; set; }

    /// <summary>
    /// Bonus bingo wins amount
    /// </summary>
    public decimal? WinsBingoBonus { get; set; }

    /// <summary>
    /// Casino bets amount
    /// </summary>
    public decimal? BetsCasino { get; set; }

    /// <summary>
    /// Casino refunds amount
    /// </summary>
    public decimal? RefundsCasino { get; set; }

    /// <summary>
    /// Casino wins amount
    /// </summary>
    public decimal? WinsCasino { get; set; }

    /// <summary>
    /// Real money casino bets amount
    /// </summary>
    public decimal? BetsCasinoReal { get; set; }

    /// <summary>
    /// Real money casino refunds amount
    /// </summary>
    public decimal? RefundsCasinoReal { get; set; }

    /// <summary>
    /// Real money casino wins amount
    /// </summary>
    public decimal? WinsCasinoReal { get; set; }

    /// <summary>
    /// Bonus casino bets amount
    /// </summary>
    public decimal? BetsCasinoBonus { get; set; }

    /// <summary>
    /// Bonus casino refunds amount
    /// </summary>
    public decimal? RefundsCasinoBonus { get; set; }

    /// <summary>
    /// Bonus casino wins amount
    /// </summary>
    public decimal? WinsCasinoBonus { get; set; }

    /// <summary>
    /// Poker bets amount
    /// </summary>
    public decimal? BetsPoker { get; set; }

    /// <summary>
    /// Poker refunds amount
    /// </summary>
    public decimal? RefundsPoker { get; set; }

    /// <summary>
    /// Poker wins amount
    /// </summary>
    public decimal? WinsPoker { get; set; }

    /// <summary>
    /// Real money poker bets amount
    /// </summary>
    public decimal? BetsPokerReal { get; set; }

    /// <summary>
    /// Real money poker refunds amount
    /// </summary>
    public decimal? RefundsPokerReal { get; set; }

    /// <summary>
    /// Real money poker wins amount
    /// </summary>
    public decimal? WinsPokerReal { get; set; }

    /// <summary>
    /// Bonus poker bets amount
    /// </summary>
    public decimal? BetsPokerBonus { get; set; }

    /// <summary>
    /// Bonus poker refunds amount
    /// </summary>
    public decimal? RefundsPokerBonus { get; set; }

    /// <summary>
    /// Bonus poker wins amount
    /// </summary>
    public decimal? WinsPokerBonus { get; set; }

    /// <summary>
    /// Games bets amount
    /// </summary>
    public decimal? BetsGames { get; set; }

    /// <summary>
    /// Games refunds amount
    /// </summary>
    public decimal? RefundsGames { get; set; }

    /// <summary>
    /// Games wins amount
    /// </summary>
    public decimal? WinsGames { get; set; }

    /// <summary>
    /// Real money games bets amount
    /// </summary>
    public decimal? BetsGamesReal { get; set; }

    /// <summary>
    /// Real money games refunds amount
    /// </summary>
    public decimal? RefundsGamesReal { get; set; }

    /// <summary>
    /// Real money games wins amount
    /// </summary>
    public decimal? WinsGamesReal { get; set; }

    /// <summary>
    /// Bonus games bets amount
    /// </summary>
    public decimal? BetsGamesBonus { get; set; }

    /// <summary>
    /// Bonus games refunds amount
    /// </summary>
    public decimal? RefundsGamesBonus { get; set; }

    /// <summary>
    /// Bonus games wins amount
    /// </summary>
    public decimal? WinsGamesBonus { get; set; }

    /// <summary>
    /// Exchange fee amount
    /// </summary>
    public decimal? ExchangeFee { get; set; }

    /// <summary>
    /// Commission amount
    /// </summary>
    public decimal? Commission { get; set; }

    /// <summary>
    /// Total transaction value
    /// </summary>
    public decimal? TransactionValue { get; set; }

    /// <summary>
    /// Real money transaction value
    /// </summary>
    public decimal? TransactionValueReal { get; set; }

    /// <summary>
    /// Bonus transaction value
    /// </summary>
    public decimal? TransactionValueBonus { get; set; }

    /// <summary>
    /// Timestamp of the last update
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Navigation property to player
    /// </summary>
    [ForeignKey("PlayerID")]
    public virtual DailyActionPlayer Player { get; set; }
}