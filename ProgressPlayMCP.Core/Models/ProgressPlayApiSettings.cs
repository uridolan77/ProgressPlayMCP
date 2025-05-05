namespace ProgressPlayMCP.Core.Models;

/// <summary>
/// Settings for the ProgressPlay API client
/// </summary>
public class ProgressPlayApiSettings
{
    /// <summary>
    /// The base URL of the ProgressPlay API
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// The username for API authentication
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password for API authentication
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The default currency to use for requests
    /// </summary>
    public string DefaultCurrency { get; set; } = "GBP";

    /// <summary>
    /// Timeout in seconds for API requests
    /// </summary>
    public int TimeoutSeconds { get; set; } = 60;
}