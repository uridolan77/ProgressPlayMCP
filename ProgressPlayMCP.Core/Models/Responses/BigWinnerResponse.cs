namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for big winners data
/// </summary>
public class BigWinnerResponse
{
    /// <summary>
    /// Transaction ID
    /// </summary>
    public long TransactionId { get; set; }
    
    /// <summary>
    /// Player ID
    /// </summary>
    public long PlayerId { get; set; }
    
    /// <summary>
    /// Player's display name (formatted for privacy)
    /// </summary>
    public string PlayerName { get; set; }
    
    /// <summary>
    /// Player's country
    /// </summary>
    public string Country { get; set; }
    
    /// <summary>
    /// Currency code
    /// </summary>
    public string Currency { get; set; }
    
    /// <summary>
    /// Type of game
    /// </summary>
    public string GameType { get; set; }
    
    /// <summary>
    /// Name of the game
    /// </summary>
    public string GameName { get; set; }
    
    /// <summary>
    /// Game ID
    /// </summary>
    public int GameId { get; set; }
    
    /// <summary>
    /// Win amount (normalized)
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Formatted amount with currency symbol
    /// </summary>
    public string AmountInCurrency { get; set; }
    
    /// <summary>
    /// Date and time of the win
    /// </summary>
    public DateTime ActionDate { get; set; }
    
    /// <summary>
    /// White label ID
    /// </summary>
    public int WhiteLabelId { get; set; }
}