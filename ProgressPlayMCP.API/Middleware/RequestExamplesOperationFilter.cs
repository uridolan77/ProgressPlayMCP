using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using ProgressPlayMCP.Core.Models.Requests;

namespace ProgressPlayMCP.API.Middleware
{
    /// <summary>
    /// Swagger operation filter that adds comprehensive request examples for each API endpoint
    /// </summary>
    public class RequestExamplesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check the controller name from the operation route
            var controllerName = context.ApiDescription.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ApiDescription.ActionDescriptor.RouteValues["action"];
            var httpMethod = context.ApiDescription.HttpMethod;

            // Only add examples for POST operations with request bodies
            if (httpMethod != "POST" || 
                operation.RequestBody?.Content == null || 
                !operation.RequestBody.Content.ContainsKey("application/json"))
            {
                return;
            }

            var mediaType = operation.RequestBody.Content["application/json"];
            mediaType.Examples ??= new Dictionary<string, OpenApiExample>();

            // Match the controller name directly
            switch (controllerName)
            {
                case "DailyActions":
                    AddDailyActionsExamples(mediaType);
                    break;
                case "PlayerSummary":
                    AddPlayerSummaryExamples(mediaType);
                    break;
                case "PlayerDetails":
                    AddPlayerDetailsExamples(mediaType);
                    break;
                case "Transactions":
                    AddTransactionsExamples(mediaType);
                    break;
                case "PlayerGames":
                    AddPlayerGamesExamples(mediaType);
                    break;
                case "IncomeAccess":
                    AddIncomeAccessExamples(mediaType);
                    break;
            }
        }

        private void AddDailyActionsExamples(OpenApiMediaType mediaType)
        {
            // Basic example with minimum required parameters
            var basicExample = new OpenApiObject
            {
                ["dateStart"] = new OpenApiString("2025/05/01"),
                ["dateEnd"] = new OpenApiString("2025/05/30"),
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2)
                }
            };

            // Full example with all possible parameters
            var fullExample = new OpenApiObject
            {
                ["dateStart"] = new OpenApiString("2025/05/01"),
                ["dateEnd"] = new OpenApiString("2025/05/30"),
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2),
                    new OpenApiInteger(3)
                },
                ["targetCurrency"] = new OpenApiString("EUR")
            };

            mediaType.Examples["Basic Request"] = new OpenApiExample
            {
                Summary = "Basic request",
                Description = "A basic request with minimum required parameters",
                Value = basicExample
            };

            mediaType.Examples["Complete Request"] = new OpenApiExample
            {
                Summary = "Complete request",
                Description = "A complete request with all possible parameters",
                Value = fullExample
            };
        }

        private void AddPlayerSummaryExamples(OpenApiMediaType mediaType)
        {
            // Basic example with minimum required parameters
            var basicExample = new OpenApiObject
            {
                ["dateStart"] = new OpenApiString("2025/05/01"),
                ["dateEnd"] = new OpenApiString("2025/05/30"),
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2)
                }
            };

            // Full example with all possible parameters
            var fullExample = new OpenApiObject
            {
                ["dateStart"] = new OpenApiString("2025/05/01"),
                ["dateEnd"] = new OpenApiString("2025/05/30"),
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2),
                    new OpenApiInteger(3)
                },
                ["targetCurrency"] = new OpenApiString("EUR")
            };

            mediaType.Examples["Basic Request"] = new OpenApiExample
            {
                Summary = "Basic request",
                Description = "A basic request with minimum required parameters",
                Value = basicExample
            };

            mediaType.Examples["Complete Request"] = new OpenApiExample
            {
                Summary = "Complete request",
                Description = "A complete request with all possible parameters",
                Value = fullExample
            };
        }

        private void AddPlayerDetailsExamples(OpenApiMediaType mediaType)
        {
            // Registration date based example
            var registrationExample = new OpenApiObject
            {
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2)
                },
                ["registrationDateStart"] = new OpenApiString("2025/01/01"),
                ["registrationDateEnd"] = new OpenApiString("2025/05/30")
            };

            // Last updated date based example
            var lastUpdatedExample = new OpenApiObject
            {
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2)
                },
                ["lastUpdatedDateStart"] = new OpenApiString("2025/04/01"),
                ["lastUpdatedDateEnd"] = new OpenApiString("2025/05/30")
            };

            // Full example with all possible parameters
            var fullExample = new OpenApiObject
            {
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2),
                    new OpenApiInteger(3)
                },
                ["registrationDateStart"] = new OpenApiString("2025/01/01"),
                ["registrationDateEnd"] = new OpenApiString("2025/05/30"),
                ["lastUpdatedDateStart"] = new OpenApiString("2025/04/01"),
                ["lastUpdatedDateEnd"] = new OpenApiString("2025/05/30")
            };

            mediaType.Examples["Registration Date Filter"] = new OpenApiExample
            {
                Summary = "Filter by registration date",
                Description = "Request filtered by player registration date range",
                Value = registrationExample
            };

            mediaType.Examples["Last Updated Date Filter"] = new OpenApiExample
            {
                Summary = "Filter by last updated date",
                Description = "Request filtered by player's last updated date range",
                Value = lastUpdatedExample
            };

            mediaType.Examples["Complete Request"] = new OpenApiExample
            {
                Summary = "Complete request",
                Description = "A complete request with all possible parameters",
                Value = fullExample
            };
        }

        private void AddTransactionsExamples(OpenApiMediaType mediaType)
        {
            // Basic example with minimum required parameters
            var basicExample = new OpenApiObject
            {
                ["dateStart"] = new OpenApiString("2025/05/01"),
                ["dateEnd"] = new OpenApiString("2025/05/30"),
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2)
                }
            };

            // Full example - for this endpoint it's the same as basic since there are no additional params
            mediaType.Examples["Standard Request"] = new OpenApiExample
            {
                Summary = "Standard request",
                Description = "A standard transactions request with required parameters",
                Value = basicExample
            };
        }

        private void AddPlayerGamesExamples(OpenApiMediaType mediaType)
        {
            // Basic example with minimum required parameters
            var basicExample = new OpenApiObject
            {
                ["dateStart"] = new OpenApiString("2025/05/01"),
                ["dateEnd"] = new OpenApiString("2025/05/30"),
                ["whiteLabels"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2)
                }
            };

            // Full example - for this endpoint it's the same as basic since there are no additional params
            mediaType.Examples["Standard Request"] = new OpenApiExample
            {
                Summary = "Standard request",
                Description = "A standard player games request with required parameters",
                Value = basicExample
            };
        }

        private void AddIncomeAccessExamples(OpenApiMediaType mediaType)
        {
            // Basic example with minimum required parameters
            var basicExample = new OpenApiObject
            {
                ["startDate"] = new OpenApiString("2025/05/01"),
                ["endDate"] = new OpenApiString("2025/05/30"),
                ["whitelabelId"] = new OpenApiInteger(1)
            };

            // Full example with all possible parameters
            var fullExample = new OpenApiObject
            {
                ["startDate"] = new OpenApiString("2025/05/01"),
                ["endDate"] = new OpenApiString("2025/05/30"),
                ["whitelabelId"] = new OpenApiInteger(1),
                ["targetCurrency"] = new OpenApiString("EUR")
            };

            mediaType.Examples["Basic Request"] = new OpenApiExample
            {
                Summary = "Basic request",
                Description = "A basic request with minimum required parameters",
                Value = basicExample
            };

            mediaType.Examples["Complete Request"] = new OpenApiExample
            {
                Summary = "Complete request",
                Description = "A complete request with all possible parameters",
                Value = fullExample
            };
        }
    }
}