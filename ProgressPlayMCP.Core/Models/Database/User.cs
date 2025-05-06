using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a user in the database
/// </summary>
public class User
{
    /// <summary>
    /// User ID (primary key)
    /// </summary>
    [Key]
    public int UserId { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    /// <summary>
    /// Password hash
    /// </summary>
    [Required]
    public string PasswordHash { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    [MaxLength(50)]
    public string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    [MaxLength(50)]
    public string LastName { get; set; }

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// Date when the user was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date when the user was last updated
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Failed login attempts
    /// </summary>
    public int FailedLoginAttempts { get; set; }

    /// <summary>
    /// Account lockout end date
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// Navigation property to user roles
    /// </summary>
    public virtual ICollection<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Navigation property to white label permissions
    /// </summary>
    public virtual ICollection<UserWhiteLabelPermission> WhiteLabelPermissions { get; set; }

    /// <summary>
    /// Navigation property to affiliate permissions
    /// </summary>
    public virtual ICollection<UserAffiliatePermission> AffiliatePermissions { get; set; }
}