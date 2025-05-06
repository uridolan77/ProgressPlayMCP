using ProgressPlayMCP.Core.Models.Auth;

namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Interface for user management operations
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Authenticate a user with username and password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <returns>User if authenticated, null otherwise</returns>
    Task<User?> AuthenticateAsync(string username, string password);

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="userRequest">User request model</param>
    /// <returns>Created user</returns>
    Task<User> CreateUserAsync(UserRequest userRequest);

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User if found, null otherwise</returns>
    Task<User?> GetUserByIdAsync(int userId);

    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns>User if found, null otherwise</returns>
    Task<User?> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of users</returns>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userRequest">User request model</param>
    /// <returns>Updated user</returns>
    Task<User> UpdateUserAsync(int userId, UserRequest userRequest);

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>True if deleted, false otherwise</returns>
    Task<bool> DeleteUserAsync(int userId);

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="currentPassword">Current password</param>
    /// <param name="newPassword">New password</param>
    /// <returns>True if changed, false otherwise</returns>
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

    /// <summary>
    /// Get user's allowed white label IDs
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of allowed white label IDs</returns>
    Task<List<int>> GetUserWhiteLabelsAsync(int userId);

    /// <summary>
    /// Get user's allowed affiliate IDs for a specific white label
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="whiteLabelId">White label ID</param>
    /// <returns>List of allowed affiliate IDs</returns>
    Task<List<string>> GetUserAffiliatesAsync(int userId, int whiteLabelId);
}