namespace ProgressPlayMCP.Core.Models;

/// <summary>
/// Settings for JWT authentication
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Secret key for signing tokens
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Issuer of the token
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audience of the token
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Access token expiration in minutes
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// Refresh token expiration in days
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;
}