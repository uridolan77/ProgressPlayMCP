namespace ProgressPlayMCP.Core.Models.Responses;

/// <summary>
/// Response model for authentication token
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token type (usually "Bearer")
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Refresh token for obtaining a new access token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}