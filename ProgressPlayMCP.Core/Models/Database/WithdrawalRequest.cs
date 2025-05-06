using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a withdrawal request in the database
/// </summary>
public class WithdrawalRequest
{
    /// <summary>
    /// Withdrawal request ID (primary key)
    /// </summary>
    [Key]
    public long WithdrawalRequestID { get; set; }

    /// <summary>
    /// Player ID (foreign key)
    /// </summary>
    public long? PlayerID { get; set; }

    /// <summary>
    /// Withdrawal amount
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Original withdrawal amount in local currency
    /// </summary>
    public decimal? OriginalAmount { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    [MaxLength(10)]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Request date
    /// </summary>
    public DateTime? RequestDate { get; set; }

    /// <summary>
    /// Processed date
    /// </summary>
    public DateTime? ProcessedDate { get; set; }

    /// <summary>
    /// Status of the withdrawal request
    /// </summary>
    [MaxLength(50)]
    public string Status { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    [MaxLength(100)]
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Payment provider
    /// </summary>
    [MaxLength(100)]
    public string PaymentProvider { get; set; }

    /// <summary>
    /// Transaction ID from payment provider
    /// </summary>
    [MaxLength(100)]
    public string ProviderTransactionID { get; set; }

    /// <summary>
    /// Admin comments
    /// </summary>
    public string AdminComments { get; set; }

    /// <summary>
    /// User ID of admin who processed the request
    /// </summary>
    public int? ProcessedByUserID { get; set; }

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
    /// Navigation property to the user who processed the request
    /// </summary>
    [ForeignKey("ProcessedByUserID")]
    public virtual User ProcessedByUser { get; set; }
}