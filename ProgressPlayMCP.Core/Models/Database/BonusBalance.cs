using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a player's bonus balance in the database
/// </summary>
public class BonusBalance
{
    /// <summary>
    /// Bonus balance ID (primary key)
    /// </summary>
    [Key]
    public long BonusBalanceID { get; set; }

    /// <summary>
    /// Player ID (foreign key)
    /// </summary>
    public long? PlayerID { get; set; }

    /// <summary>
    /// Bonus type
    /// </summary>
    [MaxLength(100)]
    public string BonusType { get; set; }

    /// <summary>
    /// Bonus amount
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Original bonus amount in local currency
    /// </summary>
    public decimal? OriginalAmount { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    [MaxLength(10)]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Wagering requirement amount
    /// </summary>
    public decimal? WageringRequirement { get; set; }

    /// <summary>
    /// Wagering completed amount
    /// </summary>
    public decimal? WageringCompleted { get; set; }

    /// <summary>
    /// Bonus expiry date
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Bonus award date
    /// </summary>
    public DateTime? AwardDate { get; set; }

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Bonus status
    /// </summary>
    [MaxLength(50)]
    public string Status { get; set; }

    /// <summary>
    /// Navigation property to player
    /// </summary>
    [ForeignKey("PlayerID")]
    public virtual DailyActionPlayer Player { get; set; }
}