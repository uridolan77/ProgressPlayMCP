using System.ComponentModel.DataAnnotations;

namespace ProgressPlayMCP.Core.Models.Reference;

/// <summary>
/// Country DTO for API responses
/// </summary>
public class CountryDto
{
    /// <summary>
    /// Country ID
    /// </summary>
    public int CountryId { get; set; }
    
    /// <summary>
    /// Country code (ISO 3166-1 alpha-2)
    /// </summary>
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Country name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Phone calling code (e.g., +1, +44)
    /// </summary>
    public string? PhoneCode { get; set; }
    
    /// <summary>
    /// Flag emoji representation
    /// </summary>
    public string? FlagEmoji { get; set; }
}

/// <summary>
/// Currency DTO for API responses
/// </summary>
public class CurrencyDto
{
    /// <summary>
    /// Currency ID
    /// </summary>
    public int CurrencyId { get; set; }
    
    /// <summary>
    /// Currency code (ISO 4217)
    /// </summary>
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Currency name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Currency symbol (e.g., $, €, £)
    /// </summary>
    public string Symbol { get; set; } = string.Empty;
    
    /// <summary>
    /// Number of decimal places typically used
    /// </summary>
    public int DecimalPlaces { get; set; } = 2;
}

/// <summary>
/// Country with Currency information DTO for API responses
/// </summary>
public class CountryWithCurrencyDto
{
    /// <summary>
    /// Country ID
    /// </summary>
    public int CountryId { get; set; }
    
    /// <summary>
    /// Country code (ISO 3166-1 alpha-2)
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Country name
    /// </summary>
    public string CountryName { get; set; } = string.Empty;
    
    /// <summary>
    /// Primary currency code
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Primary currency name
    /// </summary>
    public string CurrencyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Primary currency symbol
    /// </summary>
    public string CurrencySymbol { get; set; } = string.Empty;
}