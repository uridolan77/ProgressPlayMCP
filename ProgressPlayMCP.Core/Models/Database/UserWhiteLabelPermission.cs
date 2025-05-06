using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents user permissions for a specific white label in the database
/// </summary>
public class UserWhiteLabelPermission
{
    /// <summary>
    /// User white label permission ID (primary key)
    /// </summary>
    [Key]
    public int UserWhiteLabelId { get; set; }

    /// <summary>
    /// User ID (foreign key)
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// White label ID
    /// </summary>
    public int WhiteLabelId { get; set; }

    /// <summary>
    /// Whether the user has read access to this white label
    /// </summary>
    public bool HasReadAccess { get; set; }

    /// <summary>
    /// Whether the user has write access to this white label
    /// </summary>
    public bool HasWriteAccess { get; set; }

    /// <summary>
    /// Date when the permission was assigned
    /// </summary>
    public DateTime AssignedDate { get; set; }

    /// <summary>
    /// User ID of the user who assigned this permission
    /// </summary>
    public int? AssignedBy { get; set; }

    /// <summary>
    /// Navigation property to user
    /// </summary>
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    /// <summary>
    /// Navigation property to assigning user
    /// </summary>
    [ForeignKey("AssignedBy")]
    public virtual User AssignedByUser { get; set; }
}