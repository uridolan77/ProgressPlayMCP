using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models;
using ProgressPlayMCP.Core.Models.Auth;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.Infrastructure.Services;

/// <summary>
/// Service for JWT token generation and validation
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<TokenService> _logger;
    private readonly Dictionary<string, (string Username, IEnumerable<string> Roles, DateTime Expiration)> _refreshTokens;
    private readonly string _connectionString;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="jwtSettings">JWT settings</param>
    /// <param name="logger">Logger</param>
    /// <param name="configuration">Configuration to get connection string</param>
    public TokenService(IOptions<JwtSettings> jwtSettings, ILogger<TokenService> logger, IConfiguration configuration)
    {
        _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _refreshTokens = new Dictionary<string, (string, IEnumerable<string>, DateTime)>();
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
            throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
    }

    /// <summary>
    /// Generate JWT token for authenticated user
    /// </summary>
    /// <param name="user">Authenticated user</param>
    /// <returns>JWT token</returns>
    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            // Add UserId as NameIdentifier claim - this is crucial for permissions
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        // Add roles as claims
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            // Also add simple "role" claim to ensure compatibility with different JWT consumers
            claims.Add(new Claim("role", role));
        }
        
        // Add allowed white labels as claims
        foreach (var whiteLabelId in user.AllowedWhiteLabels)
        {
            claims.Add(new Claim("WhiteLabel", whiteLabelId.ToString()));
        }
        
        // Add allowed affiliates as claims
        foreach (var whiteLabelAffiliates in user.AllowedAffiliates)
        {
            int whiteLabelId = whiteLabelAffiliates.Key;
            foreach (var affiliateId in whiteLabelAffiliates.Value)
            {
                claims.Add(new Claim($"Affiliate:{whiteLabelId}", affiliateId));
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Generate a token for a user
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="roles">User roles</param>
    /// <returns>Token response with access and refresh tokens</returns>
    public TokenResponse GenerateToken(string username, IEnumerable<string> roles)
    {
        _logger.LogInformation("Generating token for user {Username}", username);

        // First, get the user ID by username to include in the token
        int? userId = null;
        try
        {
            // Try to look up the user ID by username using the injected connection string
            using var conn = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
            conn.Open();
            
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT UserId FROM Users WHERE Username = @Username";
            cmd.Parameters.AddWithValue("@Username", username);
            
            var result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                userId = Convert.ToInt32(result);
                _logger.LogInformation("Found user ID {UserId} for username {Username}", userId, username);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user ID for token generation");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add UserId claim if available
        if (userId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));
        }

        // Add roles to claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            // Also add simple "role" claim to ensure compatibility
            claims.Add(new Claim("role", role));
        }

        var tokenExpirationTime = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenExpirationTime,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpirationTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        
        // Store refresh token
        _refreshTokens[refreshToken] = (username, roles, refreshTokenExpirationTime);

        return new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken,
            ExpiresIn = _jwtSettings.AccessTokenExpirationMinutes * 60,
            TokenType = "Bearer"
        };
    }

    /// <summary>
    /// Generate a new access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>New token response</returns>
    public TokenResponse RefreshToken(string refreshToken)
    {
        _logger.LogInformation("Refreshing token");

        if (!_refreshTokens.TryGetValue(refreshToken, out var tokenInfo))
        {
            _logger.LogWarning("Invalid refresh token");
            throw new SecurityTokenException("Invalid refresh token");
        }

        var (username, roles, expiration) = tokenInfo;

        if (expiration < DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token expired");
            _refreshTokens.Remove(refreshToken);
            throw new SecurityTokenException("Refresh token expired");
        }

        // Remove old refresh token
        _refreshTokens.Remove(refreshToken);

        // Generate new tokens
        return GenerateToken(username, roles);
    }

    /// <summary>
    /// Validate a token
    /// </summary>
    /// <param name="token">Token to validate</param>
    /// <returns>True if the token is valid, false otherwise</returns>
    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token validation failed");
            return false;
        }
    }

    /// <summary>
    /// Generate a random refresh token
    /// </summary>
    /// <returns>Refresh token string</returns>
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}