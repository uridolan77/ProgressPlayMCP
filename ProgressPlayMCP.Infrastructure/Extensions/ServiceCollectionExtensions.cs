using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models;
using ProgressPlayMCP.Infrastructure.Clients;
using ProgressPlayMCP.Infrastructure.Services;

namespace ProgressPlayMCP.Infrastructure.Extensions;

/// <summary>
/// Extensions for registering infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the infrastructure services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The updated service collection</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add settings
        services.Configure<ProgressPlayApiSettings>(configuration.GetSection("ProgressPlayApi"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Add HTTP client
        services.AddHttpClient<IProgressPlayApiClient, ProgressPlayApiClient>();

        // Add services
        services.AddSingleton<IApiExceptionHandler, ApiExceptionHandler>();
        services.AddSingleton<IDateValidator, DateValidator>();
        services.AddSingleton<ITokenService, TokenService>();
        
        // Add MCP services
        services.AddSingleton<IMcpService, McpService>();
        
        // Register the model connector with the appropriate implementation
        // In a real application, you might have multiple model connectors and choose based on configuration
        services.AddSingleton<IModelConnector, DummyModelConnector>();

        return services;
    }
}