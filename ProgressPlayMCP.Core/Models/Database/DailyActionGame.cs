using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a game played by a player in the daily actions database
/// </summary>
public class DailyActionGame
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
    /// Game ID (foreign key)
    /// </summary>
    public int? GameID { get; set; }

    /// <summary>
    /// Game date
    /// </summary>
    public DateTime? GameDate { get; set; }

    /// <summary>
    /// Platform where game was played
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// Real money bet amount
    /// </summary>
    public decimal? RealBetAmount { get; set; }

    /// <summary>
    /// Real money win amount
    /// </summary>
    public decimal? RealWinAmount { get; set; }

    /// <summary>
    /// Bonus bet amount
    /// </summary>
    public decimal? BonusBetAmount { get; set; }

    /// <summary>
    /// Bonus win amount
    /// </summary>
    public decimal? BonusWinAmount { get; set; }

    /// <summary>
    /// Net gaming revenue
    /// </summary>
    public decimal? NetGamingRevenue { get; set; }

    /// <summary>
    /// Original real bet amount in local currency
    /// </summary>
    public decimal? RealBetAmountOriginal { get; set; }

    /// <summary>
    /// Original real win amount in local currency
    /// </summary>
    public decimal? RealWinAmountOriginal { get; set; }

    /// <summary>
    /// Original bonus bet amount in local currency
    /// </summary>
    public decimal? BonusBetAmountOriginal { get; set; }

    /// <summary>
    /// Original bonus win amount in local currency
    /// </summary>
    public decimal? BonusWinAmountOriginal { get; set; }

    /// <summary>
    /// Original net gaming revenue in local currency
    /// </summary>
    public decimal? NetGamingRevenueOriginal { get; set; }

    /// <summary>
    /// Update date
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    /// <summary>
    /// Number of real bets
    /// </summary>
    public int? NumberofRealBets { get; set; }

    /// <summary>
    /// Number of bonus bets
    /// </summary>
    public int? NumberofBonusBets { get; set; }

    /// <summary>
    /// Number of real wins
    /// </summary>
    public int? NumberofRealWins { get; set; }

    /// <summary>
    /// Number of bonus wins
    /// </summary>
    public int? NumberofBonusWins { get; set; }

    /// <summary>
    /// Number of sessions
    /// </summary>
    public int? NumberofSessions { get; set; }

    /// <summary>
    /// Navigation property to player
    /// </summary>
    [ForeignKey("PlayerID")]
    public virtual DailyActionPlayer Player { get; set; }

    /// <summary>
    /// Navigation property to game
    /// </summary>
    [ForeignKey("GameID")]
    public virtual Game Game { get; set; }
}