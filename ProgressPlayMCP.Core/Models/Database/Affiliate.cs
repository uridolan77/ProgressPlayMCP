using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents an affiliate in the database
/// </summary>
public class Affiliate
{
    /// <summary>
    /// Affiliate ID (primary key)
    /// </summary>
    [Key]
    public int AffiliateID { get; set; }

    /// <summary>
    /// Affiliate code
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string AffiliateCode { get; set; }

    /// <summary>
    /// Affiliate name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// White label ID
    /// </summary>
    public int WhiteLabelID { get; set; }

    /// <summary>
    /// Status of the affiliate
    /// </summary>
    [MaxLength(50)]
    public string Status { get; set; }

    /// <summary>
    /// Commission rate
    /// </summary>
    public decimal? CommissionRate { get; set; }

    /// <summary>
    /// Contact email
    /// </summary>
    [MaxLength(100)]
    public string ContactEmail { get; set; }

    /// <summary>
    /// Contact name
    /// </summary>
    [MaxLength(100)]
    public string ContactName { get; set; }

    /// <summary>
    /// Created date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Navigation property to white label
    /// </summary>
    [ForeignKey("WhiteLabelID")]
    public virtual WhiteLabel WhiteLabel { get; set; }

    /// <summary>
    /// Navigation property to user affiliate permissions
    /// </summary>
    public virtual ICollection<UserAffiliatePermission> UserPermissions { get; set; }
}