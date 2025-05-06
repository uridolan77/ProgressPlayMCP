using System.ComponentModel.DataAnnotations;

namespace ProgressPlayMCP.Core.Models.Reference;

/// <summary>
/// Maps countries to their currencies
/// </summary>
public class CountryCurrency
{
    /// <summary>
    /// CountryCurrency mapping ID
    /// </summary>
    public int CountryCurrencyId { get; set; }
    
    /// <summary>
    /// Country ID (foreign key to Country)
    /// </summary>
    public int CountryId { get; set; }
    
    /// <summary>
    /// Currency ID (foreign key to Currency)
    /// </summary>
    public int CurrencyId { get; set; }
    
    /// <summary>
    /// Whether this is the primary/default currency for the country
    /// </summary>
    public bool IsPrimary { get; set; } = true;
    
    /// <summary>
    /// Navigation property to associated Country
    /// </summary>
    public Country Country { get; set; } = null!;
    
    /// <summary>
    /// Navigation property to associated Currency
    /// </summary>
    public Currency Currency { get; set; } = null!;
}