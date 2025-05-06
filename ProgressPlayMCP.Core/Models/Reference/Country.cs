using System.ComponentModel.DataAnnotations;

namespace ProgressPlayMCP.Core.Models.Reference;

/// <summary>
/// Country entity model
/// </summary>
public class Country
{
    /// <summary>
    /// Country ID
    /// </summary>
    public int CountryId { get; set; }
    
    /// <summary>
    /// Country code (ISO 3166-1 alpha-2)
    /// </summary>
    [Required]
    [StringLength(2)]
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Country name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Three-letter country code (ISO 3166-1 alpha-3)
    /// </summary>
    [StringLength(3)]
    public string? Alpha3Code { get; set; }
    
    /// <summary>
    /// Numeric country code (ISO 3166-1 numeric)
    /// </summary>
    public int? NumericCode { get; set; }
    
    /// <summary>
    /// Phone calling code (e.g., +1, +44)
    /// </summary>
    [StringLength(10)]
    public string? PhoneCode { get; set; }
    
    /// <summary>
    /// Flag emoji representation
    /// </summary>
    [StringLength(10)]
    public string? FlagEmoji { get; set; }
    
    /// <summary>
    /// Region (e.g., Europe, Asia, Americas)
    /// </summary>
    [StringLength(50)]
    public string? Region { get; set; }
    
    /// <summary>
    /// Sub-region (e.g., Northern Europe, Eastern Asia)
    /// </summary>
    [StringLength(50)]
    public string? SubRegion { get; set; }
    
    /// <summary>
    /// Whether the country is active in the system
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Display order for UI sorting
    /// </summary>
    public int? DisplayOrder { get; set; }
}