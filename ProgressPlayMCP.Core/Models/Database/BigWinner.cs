using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a big winner record in the database
/// </summary>
public class BigWinner
{
    /// <summary>
    /// Big winner ID (primary key)
    /// </summary>
    [Key]
    public long BigWinnerID { get; set; }

    /// <summary>
    /// Player ID (foreign key)
    /// </summary>
    public long? PlayerID { get; set; }

    /// <summary>
    /// Game ID (foreign key)
    /// </summary>
    public int? GameID { get; set; }

    /// <summary>
    /// Win date
    /// </summary>
    public DateTime? WinDate { get; set; }

    /// <summary>
    /// Win amount
    /// </summary>
    public decimal? WinAmount { get; set; }

    /// <summary>
    /// Original win amount in local currency
    /// </summary>
    public decimal? OriginalWinAmount { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    [MaxLength(10)]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Bet amount
    /// </summary>
    public decimal? BetAmount { get; set; }

    /// <summary>
    /// Platform where win occurred
    /// </summary>
    [MaxLength(50)]
    public string Platform { get; set; }

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

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