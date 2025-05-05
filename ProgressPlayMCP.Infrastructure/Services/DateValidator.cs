using System.Globalization;
using Microsoft.Extensions.Logging;
using ProgressPlayMCP.Core.Interfaces;

namespace ProgressPlayMCP.Infrastructure.Services;

/// <summary>
/// Service for validating dates
/// </summary>
public class DateValidator : IDateValidator
{
    private readonly ILogger<DateValidator> _logger;
    private const string DateFormat = "yyyy/MM/dd";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public DateValidator(ILogger<DateValidator> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Validate a date string in the format YYYY/MM/DD
    /// </summary>
    /// <param name="dateString">The date string to validate</param>
    /// <returns>True if the date is valid, false otherwise</returns>
    public bool IsValidDate(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            _logger.LogWarning("Date string is null or empty");
            return false;
        }

        var isValid = DateTime.TryParseExact(
            dateString,
            DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);

        if (!isValid)
        {
            _logger.LogWarning("Invalid date format: {DateString}. Expected format: YYYY/MM/DD", dateString);
        }

        return isValid;
    }

    /// <summary>
    /// Validate two dates to ensure they form a valid range
    /// </summary>
    /// <param name="startDate">The start date string</param>
    /// <param name="endDate">The end date string</param>
    /// <returns>True if the date range is valid, false otherwise</returns>
    public bool IsValidDateRange(string startDate, string endDate)
    {
        if (!IsValidDate(startDate) || !IsValidDate(endDate))
        {
            return false;
        }

        var start = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
        var end = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture);

        if (start > end)
        {
            _logger.LogWarning("Start date {StartDate} is after end date {EndDate}", startDate, endDate);
            return false;
        }

        // Check if the date range is too large (optional, can be adjusted based on requirements)
        var daysDifference = (end - start).TotalDays;
        if (daysDifference > 31)
        {
            _logger.LogWarning("Date range of {Days} days exceeds the recommended maximum of 31 days", daysDifference);
            // We're returning true but logging a warning as this might be a performance concern
            // but not necessarily an invalid request
        }

        return true;
    }

    /// <summary>
    /// Format a date as a string in the format YYYY/MM/DD
    /// </summary>
    /// <param name="date">The date to format</param>
    /// <returns>The formatted date string</returns>
    public string FormatDate(DateTime date)
    {
        return date.ToString(DateFormat, CultureInfo.InvariantCulture);
    }
}