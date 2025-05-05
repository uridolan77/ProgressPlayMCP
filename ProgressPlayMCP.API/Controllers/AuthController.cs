using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;
using System.IdentityModel.Tokens.Jwt;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for authentication
/// </summary>
[AllowAnonymous]
public class AuthController : BaseController
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
    
    // In a real application, you would use a user service or repository
    // This is a simple example with hardcoded credentials
    private readonly Dictionary<string, (string PasswordHash, IEnumerable<string> Roles)> _users = new()
    {
        { "admin", ("AQAAAAIAAYagAAAAECekFxNgbQx+Pq4ahq4kFIi7KN2SUIQEDIIKePUXgdLjtQ==", new[] { "Admin" }) },
        { "user", ("AQAAAAIAAYagAAAAEBvxoLNa7fF9S4wxNi3QbDYXRBXA2h/s/tHL88ZOIqKMNl==", new[] { "User" }) }
    };

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tokenService">Token service</param>
    /// <param name="logger">Logger</param>
    public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Login and get an authentication token
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns>Token response</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Login attempt for user {Username}", request.Username);

            // In a real application, you would validate credentials against a database
            // For this example, we're using hardcoded credentials
            if (!_users.TryGetValue(request.Username, out var userInfo))
            {
                _logger.LogWarning("Login failed: user {Username} not found", request.Username);
                return Unauthorized("Invalid username or password");
            }

            // In a real application, you would use a proper password hasher
            // For this example, we're doing a simple string comparison
            if (request.Password != "password") // Simple check for demo purposes
            {
                _logger.LogWarning("Login failed: invalid password for user {Username}", request.Username);
                return Unauthorized("Invalid username or password");
            }

            var token = _tokenService.GenerateToken(request.Username, userInfo.Roles);
            
            _logger.LogInformation("Login successful for user {Username}", request.Username);
            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during login");
        }
    }

    /// <summary>
    /// Refresh an access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>New token response</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            _logger.LogInformation("Token refresh attempt");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required");
            }

            var token = _tokenService.RefreshToken(refreshToken);
            
            _logger.LogInformation("Token refresh successful");
            return Ok(token);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning(ex, "Token refresh failed: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during token refresh");
        }
    }

    /// <summary>
    /// Validate a token
    /// </summary>
    /// <param name="token">Token to validate</param>
    /// <returns>Validation result</returns>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ValidateToken([FromBody] string token)
    {
        try
        {
            _logger.LogInformation("Token validation attempt");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required");
            }

            var isValid = _tokenService.ValidateToken(token);
            
            _logger.LogInformation("Token validation result: {IsValid}", isValid);
            return Ok(isValid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token validation: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during token validation");
        }
    }
}