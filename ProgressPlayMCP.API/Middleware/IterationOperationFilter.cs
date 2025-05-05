using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace ProgressPlayMCP.API.Middleware
{
    /// <summary>
    /// Swagger operation filter that adds a "Continue to iterate?" button for endpoints
    /// </summary>
    public class IterationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Add a parameter for the "Continue" feature
            operation.Parameters ??= new List<OpenApiParameter>();

            // Check if this is a POST or PUT method where we'd want to offer continuation
            var httpMethod = context.ApiDescription.HttpMethod?.ToUpper();
            if (httpMethod == "POST" || httpMethod == "PUT")
            {
                // Add an example value that indicates this is a continuation request
                if (operation.RequestBody?.Content != null && 
                    operation.RequestBody.Content.ContainsKey("application/json"))
                {
                    var mediaType = operation.RequestBody.Content["application/json"];
                    
                    // Add an example showing the continuation format
                    mediaType.Examples ??= new Dictionary<string, OpenApiExample>();
                    
                    // Create proper OpenApiObject instead of using JsonElement
                    var exampleObject = new OpenApiObject
                    {
                        ["continueIteration"] = new OpenApiBoolean(true),
                        ["additionalInput"] = new OpenApiString("Your further instructions here...")
                    };
                    
                    mediaType.Examples["Continue"] = new OpenApiExample
                    {
                        Summary = "Continue to iterate?",
                        Description = "Use this example to continue the conversation with the API",
                        Value = exampleObject
                    };
                }
                
                // Add a description to help users understand the continuation feature
                operation.Description = string.IsNullOrEmpty(operation.Description)
                    ? "This endpoint supports iteration. Click the 'Continue' example to continue the conversation."
                    : $"{operation.Description}\n\nThis endpoint supports iteration. Click the 'Continue' example to continue the conversation.";
            }
        }
    }
}