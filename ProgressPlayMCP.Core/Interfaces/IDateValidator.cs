namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Interface for date validation
/// </summary>
public interface IDateValidator
{
    /// <summary>
    /// Validate a date string in the format YYYY/MM/DD
    /// </summary>
    /// <param name="dateString">The date string to validate</param>
    /// <returns>True if the date is valid, false otherwise</returns>
    bool IsValidDate(string dateString);

    /// <summary>
    /// Validate two dates to ensure they form a valid range
    /// </summary>
    /// <param name="startDate">The start date string</param>
    /// <param name="endDate">The end date string</param>
    /// <returns>True if the date range is valid, false otherwise</returns>
    bool IsValidDateRange(string startDate, string endDate);

    /// <summary>
    /// Format a date as a string in the format YYYY/MM/DD
    /// </summary>
    /// <param name="date">The date to format</param>
    /// <returns>The formatted date string</returns>
    string FormatDate(DateTime date);
}