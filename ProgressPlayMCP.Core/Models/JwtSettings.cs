namespace ProgressPlayMCP.Core.Models;

/// <summary>
/// JWT configuration settings
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Secret key for token signing
    /// </summary>
    public string Secret { get; set; } = string.Empty;
    
    /// <summary>
    /// Issuer of the JWT token
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
    
    /// <summary>
    /// Audience of the JWT token
    /// </summary>
    public string Audience { get; set; } = string.Empty;
    
    /// <summary>
    /// Expiration time in hours for the token
    /// </summary>
    public int ExpirationHours { get; set; } = 2;
    
    /// <summary>
    /// Access token expiration time in minutes
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 30;
    
    /// <summary>
    /// Refresh token expiration time in days
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;
}