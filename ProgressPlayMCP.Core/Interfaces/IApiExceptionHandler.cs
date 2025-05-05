namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Interface for API exception handling
/// </summary>
public interface IApiExceptionHandler
{
    /// <summary>
    /// Handle exceptions from API calls
    /// </summary>
    /// <param name="exception">The exception to handle</param>
    /// <returns>A custom exception with more details</returns>
    Exception HandleException(Exception exception);
}