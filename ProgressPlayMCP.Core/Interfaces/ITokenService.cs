using ProgressPlayMCP.Core.Models.Auth;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Interface for JWT token operations
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generate JWT token for authenticated user
    /// </summary>
    /// <param name="user">Authenticated user</param>
    /// <returns>JWT token</returns>
    string GenerateJwtToken(User user);

    /// <summary>
    /// Generate a token for a user
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="roles">User roles</param>
    /// <returns>Token response with access and refresh tokens</returns>
    TokenResponse GenerateToken(string username, IEnumerable<string> roles);

    /// <summary>
    /// Generate a new access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>New token response</returns>
    TokenResponse RefreshToken(string refreshToken);

    /// <summary>
    /// Validate a token
    /// </summary>
    /// <param name="token">Token to validate</param>
    /// <returns>True if the token is valid, false otherwise</returns>
    bool ValidateToken(string token);
}