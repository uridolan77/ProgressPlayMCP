using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ProgressPlayMCP.API.Middleware;

/// <summary>
/// Adds API Gateway documentation for consolidated endpoints
/// </summary>
public class ApiGatewayDocumentationFilter : IOperationFilter
{
    /// <summary>
    /// Apply the filter to enhance documentation for API Gateway endpoints
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        
        // Check if this is an API Gateway endpoint
        if (apiDescription.RelativePath?.StartsWith("api/gateway/") == true)
        {
            // Add a note about the consolidated API structure
            operation.Description = 
                (operation.Description ?? string.Empty) + 
                "\n\n**Note:** This endpoint is part of the consolidated API Gateway which provides a unified access point for all data endpoints.";
            
            // Replace existing "APIGateway" tag with "API Gateway" to avoid duplication
            // First, remove any existing tag related to API Gateway
            var existingTags = operation.Tags.ToList();
            operation.Tags.Clear();
            
            // Add only the "API Gateway" tag and keep other unrelated tags
            var tagAdded = false;
            foreach (var tag in existingTags)
            {
                if (tag.Name == "APIGateway")
                {
                    if (!tagAdded)
                    {
                        operation.Tags.Add(new OpenApiTag { Name = "API Gateway" });
                        tagAdded = true;
                    }
                }
                else
                {
                    operation.Tags.Add(tag);
                }
            }
            
            // Ensure the tag is added even if there was no "APIGateway" tag
            if (!tagAdded)
            {
                operation.Tags.Add(new OpenApiTag { Name = "API Gateway" });
            }
        }
    }
}