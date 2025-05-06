using System.ComponentModel.DataAnnotations;

namespace ProgressPlayMCP.Core.Models.Reference;

/// <summary>
/// Currency entity model
/// </summary>
public class Currency
{
    /// <summary>
    /// Currency ID
    /// </summary>
    public int CurrencyId { get; set; }
    
    /// <summary>
    /// Currency code (ISO 4217)
    /// </summary>
    [Required]
    [StringLength(3)]
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Currency name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Currency symbol (e.g., $, €, £)
    /// </summary>
    [StringLength(10)]
    public string Symbol { get; set; } = string.Empty;
    
    /// <summary>
    /// Number of decimal places typically used
    /// </summary>
    public int DecimalPlaces { get; set; } = 2;
    
    /// <summary>
    /// Numeric currency code (ISO 4217)
    /// </summary>
    public int? NumericCode { get; set; }
    
    /// <summary>
    /// Whether the currency is active in the system
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Display order for UI sorting
    /// </summary>
    public int? DisplayOrder { get; set; }
    
    /// <summary>
    /// Whether the currency symbol should be displayed before the amount
    /// </summary>
    public bool SymbolPrecedesAmount { get; set; } = true;
    
    /// <summary>
    /// Whether a space should be included between the symbol and amount
    /// </summary>
    public bool HasSpaceBetweenSymbolAndAmount { get; set; } = false;
}