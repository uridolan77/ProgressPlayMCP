using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.Auth;
using ProgressPlayMCP.Core.Models.Responses;
using System.Security.Claims;
using BCrypt.Net;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for authentication and user management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor
    /// </summary>
    public AuthController(
        IUserService userService,
        ITokenService tokenService,
        ILogger<AuthController> logger,
        IConfiguration configuration)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Authenticate a user
    /// </summary>
    /// <param name="loginRequest">Login request</param>
    /// <returns>Authentication response with token</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {Username}", loginRequest.Username);

            var user = await _userService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            
            if (user == null)
            {
                _logger.LogWarning("Login failed for user: {Username}", loginRequest.Username);
                return Unauthorized(new { message = "Username or password is incorrect" });
            }

            var token = _tokenService.GenerateJwtToken(user);
            var tokenExpiration = DateTime.UtcNow.AddHours(2); // Token expiration time

            return Ok(new LoginResponse
            {
                Token = token,
                Expiration = tokenExpiration,
                Username = user.Username,
                Roles = user.Roles,
                AllowedWhiteLabels = user.AllowedWhiteLabels
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Username}", loginRequest.Username);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    /// <summary>
    /// TEMPORARY: Reset the admin password to the default from seed data
    /// Remove this method after use!
    /// </summary>
    /// <returns>Success or error message</returns>
    [HttpGet("reset-admin-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetAdminPassword()
    {
        try
        {
            // This is the hash for "Admin@123456" from the seed script
            string seedPasswordHash = "AQAAAAIAAYagAAAAEKGIieH3g4qzx9nbEyfbL5xrJEO7Pca4TGxn829fSiZj1QPNM6mF4/+rSm+1RDVW8w==";
            
            // Direct database update using ADO.NET to avoid validation/hashing logic in service layer
            using var conn = new Microsoft.Data.SqlClient.SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();
            
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE dbo.Users SET PasswordHash = @PasswordHash WHERE Username = 'admin'";
            cmd.Parameters.AddWithValue("@PasswordHash", seedPasswordHash);
            
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            
            if (rowsAffected > 0)
            {
                _logger.LogWarning("Admin password was reset to default seed value");
                return Ok(new { message = "Admin password reset to 'Admin@123456'" });
            }
            
            return NotFound(new { message = "Admin user not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting admin password");
            return StatusCode(500, new { message = "An error occurred while resetting the admin password" });
        }
    }

    /// <summary>
    /// TEMPORARY: Reset admin password to use BCrypt (directly updates the database)
    /// </summary>
    [HttpGet("reset-admin-password-bcrypt")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetAdminPasswordBCrypt()
    {
        try
        {
            // Generate a fresh BCrypt hash for "Admin@123456" right now, using our exact implementation
            string bcryptHash = BCrypt.Net.BCrypt.HashPassword("Admin@123456", BCrypt.Net.BCrypt.GenerateSalt(12));
            
            // Log the hash we're about to use
            _logger.LogWarning("Generated fresh BCrypt hash for Admin@123456: {Hash}", bcryptHash);
            
            // Direct database update using ADO.NET
            using var conn = new Microsoft.Data.SqlClient.SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();
            
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE dbo.Users SET PasswordHash = @PasswordHash WHERE Username = 'admin'";
            cmd.Parameters.AddWithValue("@PasswordHash", bcryptHash);
            
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            
            if (rowsAffected > 0)
            {
                _logger.LogWarning("Admin password was reset with a fresh BCrypt hash");
                return Ok(new { message = "Admin password reset to 'Admin@123456' with fresh BCrypt hash", hash = bcryptHash });
            }
            
            return NotFound(new { message = "Admin user not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting admin password with BCrypt");
            return StatusCode(500, new { message = "An error occurred while resetting the admin password" });
        }
    }

    /// <summary>
    /// TEMPORARY: Generate a BCrypt hash for a password and log it
    /// </summary>
    [HttpGet("generate-hash")]
    [AllowAnonymous]
    public IActionResult GenerateHash([FromQuery] string password = "Admin@123456")
    {
        try
        {
            // Generate hash using the same method as in UserService
            var hash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
            
            // Log the hash
            _logger.LogWarning("Generated hash for password '{Password}': {Hash}", password, hash);
            
            return Ok(new { Hash = hash });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating hash");
            return StatusCode(500, new { message = "An error occurred while generating hash" });
        }
    }

    /// <summary>
    /// Get all users (admin only)
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            
            var userResponses = users.Select(user => new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                CreatedDate = user.CreatedDate,
                Roles = user.Roles,
                // For simplicity, we're not returning WhiteLabelPermissions here,
                // but in a real app you would map these from user.AllowedWhiteLabels and user.AllowedAffiliates
            });

            return Ok(userResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, new { message = "An error occurred while retrieving users" });
        }
    }

    /// <summary>
    /// Get user by ID (admin only)
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("users/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> GetUserById(int userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {userId} not found" });
            }

            var userResponse = new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                CreatedDate = user.CreatedDate,
                Roles = user.Roles
                // WhiteLabelPermissions would be mapped here
            };

            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with ID {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while retrieving the user" });
        }
    }

    /// <summary>
    /// Create a new user (admin only)
    /// </summary>
    /// <param name="userRequest">User request</param>
    /// <returns>Created user</returns>
    [HttpPost("users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserRequest userRequest)
    {
        try
        {
            var user = await _userService.CreateUserAsync(userRequest);

            var userResponse = new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                CreatedDate = user.CreatedDate,
                Roles = user.Roles
                // WhiteLabelPermissions would be mapped here
            };

            return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, userResponse);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error while creating user {Username}", userRequest.Username);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {Username}", userRequest.Username);
            return StatusCode(500, new { message = "An error occurred while creating the user" });
        }
    }

    /// <summary>
    /// Update an existing user (admin only)
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userRequest">User request</param>
    /// <returns>Updated user</returns>
    [HttpPut("users/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> UpdateUser(int userId, [FromBody] UserRequest userRequest)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(userId, userRequest);

            var userResponse = new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                CreatedDate = user.CreatedDate,
                Roles = user.Roles
                // WhiteLabelPermissions would be mapped here
            };

            return Ok(userResponse);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error while updating user with ID {UserId}", userId);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while updating the user" });
        }
    }

    /// <summary>
    /// Delete a user (admin only)
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>No content</returns>
    [HttpDelete("users/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(userId);
            
            if (!result)
            {
                return NotFound(new { message = $"User with ID {userId} not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while deleting the user" });
        }
    }

    /// <summary>
    /// Change password (requires authentication)
    /// </summary>
    /// <param name="changePasswordRequest">Change password request</param>
    /// <returns>No content</returns>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        try
        {
            // Get the user ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user credentials" });
            }

            var result = await _userService.ChangePasswordAsync(
                userId, 
                changePasswordRequest.CurrentPassword, 
                changePasswordRequest.NewPassword);
            
            if (!result)
            {
                return BadRequest(new { message = "Current password is incorrect" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            return StatusCode(500, new { message = "An error occurred while changing the password" });
        }
    }

    /// <summary>
    /// Get current user's profile
    /// </summary>
    /// <returns>User profile</returns>
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetProfile()
    {
        try
        {
            var username = User.Identity?.Name;
            
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Invalid user credentials" });
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userResponse = new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                CreatedDate = user.CreatedDate,
                Roles = user.Roles
                // WhiteLabelPermissions would be mapped here
            };

            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile");
            return StatusCode(500, new { message = "An error occurred while retrieving the user profile" });
        }
    }
}