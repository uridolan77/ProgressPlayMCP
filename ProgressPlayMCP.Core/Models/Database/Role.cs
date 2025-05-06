using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a role in the database
/// </summary>
public class Role
{
    /// <summary>
    /// Role ID (primary key)
    /// </summary>
    [Key]
    public int RoleId { get; set; }

    /// <summary>
    /// Role name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// Role description
    /// </summary>
    [MaxLength(255)]
    public string Description { get; set; }

    /// <summary>
    /// Date when the role was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Navigation property to user roles
    /// </summary>
    public virtual ICollection<UserRole> UserRoles { get; set; }
}