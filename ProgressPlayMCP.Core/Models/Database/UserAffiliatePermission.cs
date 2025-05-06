using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents user permissions for a specific affiliate in the database
/// </summary>
public class UserAffiliatePermission
{
    /// <summary>
    /// User affiliate permission ID (primary key)
    /// </summary>
    [Key]
    public int UserAffiliateId { get; set; }

    /// <summary>
    /// User ID (foreign key)
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// White label ID
    /// </summary>
    public int WhiteLabelId { get; set; }

    /// <summary>
    /// Affiliate ID
    /// </summary>
    [MaxLength(100)]
    public string AffiliateID { get; set; }

    /// <summary>
    /// Whether the user has read access to this affiliate
    /// </summary>
    public bool HasReadAccess { get; set; }

    /// <summary>
    /// Whether the user has write access to this affiliate
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