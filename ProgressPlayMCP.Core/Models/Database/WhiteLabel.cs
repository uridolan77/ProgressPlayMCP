using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a white label (casino brand) in the database
/// </summary>
public class WhiteLabel
{
    /// <summary>
    /// White label ID (primary key)
    /// </summary>
    [Key]
    public int LabelID { get; set; }
    
    /// <summary>
    /// White label name (casino name)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    /// <summary>
    /// White label status (Active, Inactive, etc.)
    /// </summary>
    [MaxLength(50)]
    public string Status { get; set; }
    
    /// <summary>
    /// White label creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }
    
    /// <summary>
    /// White label last updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
    
    /// <summary>
    /// Currency code used by this white label
    /// </summary>
    [MaxLength(10)]
    public string DefaultCurrency { get; set; }
    
    /// <summary>
    /// URL to the white label's logo
    /// </summary>
    [MaxLength(255)]
    public string LogoUrl { get; set; }
    
    /// <summary>
    /// Primary contact email for the white label
    /// </summary>
    [MaxLength(100)]
    public string ContactEmail { get; set; }

    /// <summary>
    /// Navigation property to user white label permissions
    /// </summary>
    public virtual ICollection<UserWhiteLabelPermission> UserPermissions { get; set; }
    
    /// <summary>
    /// Navigation property to user affiliate permissions
    /// </summary>
    public virtual ICollection<UserAffiliatePermission> AffiliatePermissions { get; set; }
}