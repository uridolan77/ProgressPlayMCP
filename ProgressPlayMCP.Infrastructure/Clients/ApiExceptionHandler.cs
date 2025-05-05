using System.Net;
using Microsoft.Extensions.Logging;
using ProgressPlayMCP.Core.Interfaces;
using System.Text.Json;

namespace ProgressPlayMCP.Infrastructure.Clients;

/// <summary>
/// Implementation of the API exception handler
/// </summary>
public class ApiExceptionHandler : IApiExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handle exceptions from API calls
    /// </summary>
    /// <param name="exception">The exception to handle</param>
    /// <returns>A custom exception with more details</returns>
    public Exception HandleException(Exception exception)
    {
        _logger.LogDebug("Handling exception of type {ExceptionType}", exception.GetType().Name);

        // Handle specific exception types
        switch (exception)
        {
            case HttpRequestException httpEx:
                return HandleHttpException(httpEx);
                
            case JsonException jsonEx:
                _logger.LogError(jsonEx, "JSON deserialization error");
                return new InvalidOperationException("Error parsing API response. The response format might have changed.", jsonEx);
                
            case TaskCanceledException tcEx:
                _logger.LogWarning(tcEx, "API request was canceled");
                return new OperationCanceledException("The API request was canceled due to a timeout or user cancellation.", tcEx);
                
            case ObjectDisposedException odEx:
                _logger.LogError(odEx, "Object disposed error");
                return new InvalidOperationException("The HTTP client was disposed before completing the request.", odEx);
                
            default:
                _logger.LogError(exception, "Unhandled exception");
                return new InvalidOperationException("An unexpected error occurred while calling the ProgressPlay API.", exception);
        }
    }

    /// <summary>
    /// Handle HTTP request exceptions
    /// </summary>
    /// <param name="exception">The HTTP exception to handle</param>
    /// <returns>A custom exception with more details</returns>
    private Exception HandleHttpException(HttpRequestException exception)
    {
        string message = "An error occurred while communicating with the ProgressPlay API";
        
        if (exception.StatusCode.HasValue)
        {
            switch (exception.StatusCode.Value)
            {
                case HttpStatusCode.Unauthorized:
                    message = "Authentication failed. Please check your API credentials.";
                    _logger.LogError(exception, "Authentication failed with ProgressPlay API");
                    break;
                    
                case HttpStatusCode.Forbidden:
                    message = "Access denied. You don't have permission to access this resource.";
                    _logger.LogError(exception, "Access forbidden to ProgressPlay API resource");
                    break;
                    
                case HttpStatusCode.NotFound:
                    message = "The requested API endpoint was not found.";
                    _logger.LogError(exception, "ProgressPlay API endpoint not found");
                    break;
                    
                case HttpStatusCode.BadRequest:
                    message = "Invalid request parameters. Please check your request data.";
                    _logger.LogError(exception, "Bad request to ProgressPlay API");
                    break;
                    
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    message = "The ProgressPlay API is currently unavailable. Please try again later.";
                    _logger.LogError(exception, "ProgressPlay API service error: {StatusCode}", exception.StatusCode);
                    break;
                    
                default:
                    message = $"HTTP error {(int)exception.StatusCode.Value} occurred while calling the ProgressPlay API.";
                    _logger.LogError(exception, "HTTP error {StatusCode} calling ProgressPlay API", exception.StatusCode);
                    break;
            }
        }
        else
        {
            // Handle network connectivity issues
            if (exception.InnerException is System.Net.Sockets.SocketException)
            {
                message = "Could not connect to the ProgressPlay API. Please check your network connection.";
                _logger.LogError(exception, "Network error connecting to ProgressPlay API");
            }
        }
        
        return new InvalidOperationException(message, exception);
    }
}