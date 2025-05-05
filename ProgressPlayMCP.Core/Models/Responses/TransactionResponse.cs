using System.Text.Json.Serialization;

namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for Transactions endpoint
/// </summary>
public class TransactionResponse
{
    [JsonPropertyName("OriginalTransactionID")]
    public long? OriginalTransactionId { get; set; }

    [JsonPropertyName("PlayerID")]
    public int? PlayerId { get; set; }

    [JsonPropertyName("WhitelabelId")]
    public int? WhitelabelId { get; set; }

    [JsonPropertyName("CasinoName")]
    public string CasinoName { get; set; } = string.Empty;

    [JsonPropertyName("TransactionDate")]
    public DateTime? TransactionDate { get; set; }

    [JsonPropertyName("TransactionType")]
    public string TransactionType { get; set; } = string.Empty;

    [JsonPropertyName("Currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("CurrencySymbol")]
    public string CurrencySymbol { get; set; } = string.Empty;

    [JsonPropertyName("TransactionOriginalAmount")]
    public decimal? TransactionOriginalAmount { get; set; }

    [JsonPropertyName("TransactionDetails")]
    public string TransactionDetails { get; set; } = string.Empty;

    [JsonPropertyName("Status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("TotalRecords")]
    public int? TotalRecords { get; set; }
}