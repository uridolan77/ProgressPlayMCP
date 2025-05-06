using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ProgressPlayMCP.Core.Models.Auth;

/// <summary>
/// User entity model
/// </summary>
public class User
{
    /// <summary>
    /// User ID
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Password hash (not returned in JSON)
    /// </summary>
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// First name
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Last name
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginDate { get; set; }
    
    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
    
    /// <summary>
    /// Number of failed login attempts
    /// </summary>
    public int FailedLoginAttempts { get; set; }
    
    /// <summary>
    /// Account lockout end date/time
    /// </summary>
    public DateTime? LockoutEnd { get; set; }
    
    /// <summary>
    /// User roles
    /// </summary>
    public List<string> Roles { get; set; } = new List<string>();
    
    /// <summary>
    /// WhiteLabel IDs the user has access to
    /// </summary>
    public List<int> AllowedWhiteLabels { get; set; } = new List<int>();
    
    /// <summary>
    /// Dictionary of WhiteLabel IDs to list of affiliate IDs the user has access to
    /// </summary>
    public Dictionary<int, List<string>> AllowedAffiliates { get; set; } = new Dictionary<int, List<string>>();
}

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Login response model
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// JWT token
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Token expiration date/time
    /// </summary>
    public DateTime Expiration { get; set; }
    
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// User roles
    /// </summary>
    public List<string> Roles { get; set; } = new List<string>();
    
    /// <summary>
    /// WhiteLabel IDs the user has access to
    /// </summary>
    public List<int> AllowedWhiteLabels { get; set; } = new List<int>();
}

/// <summary>
/// User create/update request model
/// </summary>
public class UserRequest
{
    /// <summary>
    /// Username
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// First name
    /// </summary>
    [StringLength(50)]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    [StringLength(50)]
    public string? LastName { get; set; }

    /// <summary>
    /// Password (required for creation, optional for updates)
    /// </summary>
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User roles
    /// </summary>
    [Required]
    public List<string> Roles { get; set; } = new List<string>();

    /// <summary>
    /// WhiteLabel IDs the user has access to
    /// </summary>
    public List<int> WhiteLabelPermissions { get; set; } = new List<int>();

    /// <summary>
    /// Dictionary of WhiteLabel IDs to list of affiliate IDs the user has access to
    /// </summary>
    public Dictionary<int, List<string>> AffiliatePermissions { get; set; } = new Dictionary<int, List<string>>();
}

/// <summary>
/// User details response model
/// </summary>
public class UserResponse
{
    /// <summary>
    /// User ID
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// First name
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Last name
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginDate { get; set; }
    
    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }
    
    /// <summary>
    /// User roles
    /// </summary>
    public List<string> Roles { get; set; } = new List<string>();
    
    /// <summary>
    /// WhiteLabel permissions
    /// </summary>
    public List<WhiteLabelPermission> WhiteLabelPermissions { get; set; } = new List<WhiteLabelPermission>();
}

/// <summary>
/// WhiteLabel permission model
/// </summary>
public class WhiteLabelPermission
{
    /// <summary>
    /// WhiteLabel ID
    /// </summary>
    public int WhiteLabelId { get; set; }
    
    /// <summary>
    /// WhiteLabel name
    /// </summary>
    public string WhiteLabelName { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the user has read access
    /// </summary>
    public bool HasReadAccess { get; set; }
    
    /// <summary>
    /// Whether the user has write access
    /// </summary>
    public bool HasWriteAccess { get; set; }
    
    /// <summary>
    /// Affiliate IDs the user has access to within this WhiteLabel
    /// </summary>
    public List<string> AffiliateIds { get; set; } = new List<string>();
}

/// <summary>
/// Change password request model
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Current password
    /// </summary>
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirm new password
    /// </summary>
    [Required]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}