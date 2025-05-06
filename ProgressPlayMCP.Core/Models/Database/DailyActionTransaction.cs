using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a transaction in the daily actions database
/// </summary>
public class DailyActionTransaction
{
    /// <summary>
    /// Transaction ID (primary key)
    /// </summary>
    [Key]
    public long? TransactionID { get; set; }

    /// <summary>
    /// Original transaction ID from source system
    /// </summary>
    public long? OriginalTransactionID { get; set; }

    /// <summary>
    /// Player ID (foreign key)
    /// </summary>
    public long? PlayerID { get; set; }

    /// <summary>
    /// Transaction date
    /// </summary>
    public DateTime? TransactionDate { get; set; }

    /// <summary>
    /// Transaction type
    /// </summary>
    public string TransactionType { get; set; }

    /// <summary>
    /// Transaction amount (in EUR)
    /// </summary>
    public decimal? TransactionAmount { get; set; }

    /// <summary>
    /// Original transaction amount (in local currency)
    /// </summary>
    public decimal? TransactionOriginalAmount { get; set; }

    /// <summary>
    /// Transaction details
    /// </summary>
    public string TransactionDetails { get; set; }

    /// <summary>
    /// Transaction sub details
    /// </summary>
    public string TransactionSubDetails { get; set; }

    /// <summary>
    /// Platform where transaction occurred
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// Transaction status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Transaction info ID
    /// </summary>
    public long? TransactionInfoID { get; set; }

    /// <summary>
    /// Transaction comments
    /// </summary>
    public string TransactionComments { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Payment provider
    /// </summary>
    public string PaymentProvider { get; set; }

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Navigation property to the player
    /// </summary>
    [ForeignKey("PlayerID")]
    public virtual DailyActionPlayer Player { get; set; }
}