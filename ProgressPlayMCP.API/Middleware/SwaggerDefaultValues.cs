using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ProgressPlayMCP.Core.Models.Auth;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ProgressPlayMCP.API.Middleware;

/// <summary>
/// Provides login request examples in Swagger UI
/// </summary>
public class SwaggerDefaultValues : IOperationFilter
{
    /// <summary>
    /// Apply the filter to set default values for requests
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        
        // Check if this is the login endpoint
        if (apiDescription.RelativePath == "api/Auth/login" && operation.RequestBody != null)
        {
            var schema = operation.RequestBody.Content["application/json"].Schema;
            
            // Set an example with admin credentials
            operation.RequestBody.Content["application/json"].Example = new OpenApiObject
            {
                ["username"] = new OpenApiString("admin"),
                ["password"] = new OpenApiString("Admin@123456")
            };
        }
    }
}