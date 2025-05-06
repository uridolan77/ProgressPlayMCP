using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a user-role association in the database
/// </summary>
public class UserRole
{
    /// <summary>
    /// User role ID (primary key)
    /// </summary>
    [Key]
    public int UserRoleId { get; set; }

    /// <summary>
    /// User ID (foreign key)
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Role ID (foreign key)
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Date when the role was assigned
    /// </summary>
    public DateTime AssignedDate { get; set; }

    /// <summary>
    /// Navigation property to user
    /// </summary>
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    /// <summary>
    /// Navigation property to role
    /// </summary>
    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; }
}