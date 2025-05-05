using System.ComponentModel.DataAnnotations;

namespace ProgressPlayMCP.Core.Models.Requests;

/// <summary>
/// Request model for user authentication
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username for authentication
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password for authentication
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}