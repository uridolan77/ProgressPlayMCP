using System.Net;
using System.Text.Json;

namespace ProgressPlayMCP.API.Middleware;

/// <summary>
/// Middleware to handle global exceptions
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next">Next middleware in the pipeline</param>
    /// <param name="logger">Logger</param>
    /// <param name="env">Environment</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Invoke the middleware
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handle an exception
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <param name="exception">The exception to handle</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errorMessage = "An unexpected error occurred.";

        // Adjust status code based on exception type
        switch (exception)
        {
            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
                break;
            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "Unauthorized access.";
                break;
            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorMessage = "The requested resource was not found.";
                break;
            case TimeoutException:
                statusCode = HttpStatusCode.GatewayTimeout;
                errorMessage = "The request timed out.";
                break;
            default:
                // Log the detailed error but return a generic message to the client
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                break;
        }

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = errorMessage,
            Details = _env.IsDevelopment() ? exception.StackTrace : null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}

/// <summary>
/// Extension methods for ExceptionHandlingMiddleware
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>
    /// Use the exception handling middleware
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The updated application builder</returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}