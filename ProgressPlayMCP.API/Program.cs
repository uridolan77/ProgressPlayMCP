using System.Reflection;
using System.Text;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProgressPlayMCP.API.Hubs;
using ProgressPlayMCP.API.Middleware;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models;
using ProgressPlayMCP.Infrastructure.Extensions;
using ProgressPlayMCP.Infrastructure.Services;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Load User Secrets in development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    Log.Information("User Secrets loaded for development environment");
}

// Configure Azure Key Vault for all environments
if (builder.Configuration.GetSection("KeyVault").Exists())
{
    var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
    if (!string.IsNullOrEmpty(keyVaultUri))
    {
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions 
        { 
            ExcludeSharedTokenCacheCredential = true 
        });
        
        builder.Configuration.AddAzureKeyVault(
            new Uri(keyVaultUri),
            credential);
        
        Log.Information("Azure Key Vault configured for {Environment} environment", 
            builder.Environment.EnvironmentName);
            
        // Update the connection string with the values from Key Vault
        ConfigureConnectionString(builder.Configuration);
    }
}

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine("logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 10 * 1024 * 1024,
        retainedFileCountLimit: 31)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register user management services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://yourdomain.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Add SignalR for WebSocket support
builder.Services.AddSignalR();

// Add API rate limiting (optional)
// builder.Services.AddRateLimiting(builder.Configuration);

// Add authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT secret is not configured."));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        // Add a 5-minute clock skew to account for server time differences
        ClockSkew = TimeSpan.FromMinutes(5),
        // Make sure role claims are properly mapped
        RoleClaimType = "role"
    };
    
    // Configure JWT Bearer to work with SignalR
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/mcphub"))
            {
                context.Token = accessToken;
            }
            
            return Task.CompletedTask;
        },
        // Add better error handling for token validation failures
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
                context.Fail("Token has expired");
                Log.Warning("Authentication failed: Token has expired");
            }
            else
            {
                Log.Warning(context.Exception, "Authentication failed: {ErrorMessage}", context.Exception.Message);
            }
            return Task.CompletedTask;
        }
    };
});

// Configure authorization policies to recognize both standard and simple role claims
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => 
        policy.RequireAssertion(context => 
            context.User.IsInRole("Admin") || 
            context.User.Claims.Any(c => c.Type == "role" && c.Value == "Admin")));
    
    options.AddPolicy("Manager", policy => 
        policy.RequireAssertion(context => 
            context.User.IsInRole("Manager") || 
            context.User.Claims.Any(c => c.Type == "role" && c.Value == "Manager")));
    
    options.AddPolicy("User", policy => 
        policy.RequireAssertion(context => 
            context.User.IsInRole("User") || 
            context.User.Claims.Any(c => c.Type == "role" && c.Value == "User")));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization for MCP messages
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProgressPlay MCP API",
        Version = "v1",
        Description = "API for accessing ProgressPlay Reports data and MCP server functionality",
        Contact = new OpenApiContact
        {
            Name = "Your Company",
            Email = "support@yourcompany.com"
        }
    });
    
    // Set up JWT authentication in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML Comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    else
    {
        Log.Warning("XML documentation file not found at {XmlPath}. Make sure <GenerateDocumentationFile>true</GenerateDocumentationFile> is in your .csproj file.", xmlPath);
    }
    
    // Register the operation filters
    c.OperationFilter<SwaggerDefaultValues>(); // Add default values filter for login
    c.OperationFilter<RequestExamplesOperationFilter>(); // Add comprehensive request examples
    c.OperationFilter<IterationOperationFilter>(); // Then add the iteration examples
    
    // Enable Swagger annotations
    c.EnableAnnotations();
});

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProgressPlay MCP API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.EnableFilter();
        c.DisplayRequestDuration();
        
        // Add custom JavaScript to auto-set bearer token
        c.InjectJavascript("/swagger/swagger-custom.js");
    });
}
else
{
    app.UseExceptionHandling();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files from wwwroot folder
app.UseSerilogRequestLogging();
app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<McpHub>("/mcphub");
app.MapHealthChecks("/health");

try
{
    Log.Information("Starting web host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

// Helper method to configure the connection string with values from Key Vault
void ConfigureConnectionString(IConfiguration configuration)
{
    try
    {
        // Get the connection string template
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Log.Information("Original connection string: {ConnectionString}", connectionString);
        
        // Log all available configuration keys to diagnose the issue
        Log.Information("Available configuration keys in root:");
        foreach (var key in ((IConfigurationRoot)configuration).Providers)
        {
            Log.Information("Provider: {ProviderType}", key.GetType().Name);
        }
        
        // Specific check for the exact Key Vault secrets we expect
        var usernameKey = "ProgressPlayDBAzure--Username";
        var passwordKey = "ProgressPlayDBAzure--Password";
        
        var username = configuration[usernameKey];
        var password = configuration[passwordKey];
        
        Log.Information("Direct access - Username from Key Vault present: {UsernameFound}", !string.IsNullOrEmpty(username));
        Log.Information("Direct access - Password from Key Vault present: {PasswordFound}", !string.IsNullOrEmpty(password));
        
        // Try alternative naming patterns
        if (string.IsNullOrEmpty(username))
        {
            Log.Information("Trying alternative naming patterns for username");
            username = configuration["ProgressPlayDBAzure:Username"] 
                    ?? configuration["ProgressPlayDBAzure--Username"] 
                    ?? configuration["progressplaydbazure--username"];
            
            if (!string.IsNullOrEmpty(username))
            {
                Log.Information("Found username with alternative naming pattern");
            }
        }
        
        if (string.IsNullOrEmpty(password))
        {
            Log.Information("Trying alternative naming patterns for password");
            password = configuration["ProgressPlayDBAzure:Password"] 
                    ?? configuration["ProgressPlayDBAzure--Password"] 
                    ?? configuration["progressplaydbazure--password"];
            
            if (!string.IsNullOrEmpty(password))
            {
                Log.Information("Found password with alternative naming pattern");
            }
        }
        
        // Last resort: manually scan all keys for anything similar
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Log.Information("Scanning all configuration keys for possible database credentials:");
            foreach (var key in ((IConfigurationRoot)configuration).AsEnumerable())
            {
                // Only log key names that might contain sensitive info, not the values
                if ((key.Key.Contains("user", StringComparison.OrdinalIgnoreCase) || 
                     key.Key.Contains("pass", StringComparison.OrdinalIgnoreCase) ||
                     key.Key.Contains("secret", StringComparison.OrdinalIgnoreCase) ||
                     key.Key.Contains("key", StringComparison.OrdinalIgnoreCase) ||
                     key.Key.Contains("vault", StringComparison.OrdinalIgnoreCase) ||
                     key.Key.Contains("conn", StringComparison.OrdinalIgnoreCase) ||
                     key.Key.Contains("azure", StringComparison.OrdinalIgnoreCase) ||
                     key.Key.Contains("db", StringComparison.OrdinalIgnoreCase) ||
                     key.Key.Contains("sql", StringComparison.OrdinalIgnoreCase)))
                {
                    Log.Information("Found potential credential key: {Key} with value present: {HasValue}", 
                        key.Key, !string.IsNullOrEmpty(key.Value));
                    
                    // Try to match username-like keys
                    if (string.IsNullOrEmpty(username) && 
                        (key.Key.Contains("user", StringComparison.OrdinalIgnoreCase) || 
                         key.Key.Contains("name", StringComparison.OrdinalIgnoreCase)) &&
                        !key.Key.Contains("pass", StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrEmpty(key.Value))
                    {
                        username = key.Value;
                        Log.Information("Using {Key} for username", key.Key);
                    }
                    
                    // Try to match password-like keys
                    if (string.IsNullOrEmpty(password) && 
                        (key.Key.Contains("pass", StringComparison.OrdinalIgnoreCase) || 
                         key.Key.Contains("secret", StringComparison.OrdinalIgnoreCase)) &&
                        !string.IsNullOrEmpty(key.Value))
                    {
                        password = key.Value;
                        Log.Information("Using {Key} for password", key.Key);
                    }
                }
            }
        }
        
        // As a last resort, check for hard-coded fallback values
        if (string.IsNullOrEmpty(username))
        {
            username = "pp-sa";
            Log.Warning("Using hard-coded username fallback value");
        }
        
        if (string.IsNullOrEmpty(password))
        {
            password = "RDlS8***********_oY5Y";
            Log.Warning("Using hard-coded password fallback value");
        }
        
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(connectionString))
        {
            Log.Information("Before replacement - connection string contains USERNAME placeholder: {ContainsUsername}", 
                connectionString.Contains("${USERNAME}"));
                
            // Replace the placeholders with actual values
            var updatedConnectionString = connectionString
                .Replace("${USERNAME}", username)
                .Replace("${PASSWORD}", password);
                
            Log.Information("After replacement - Updated connection string contains USERNAME placeholder: {ContainsUsername}", 
                updatedConnectionString.Contains("${USERNAME}"));
            
            // Update the connection string in the configuration
            var configurationRoot = (IConfigurationRoot)configuration;
            configurationRoot.GetSection("ConnectionStrings")["DefaultConnection"] = updatedConnectionString;
            
            // Verify the update worked
            var finalConnectionString = configuration.GetConnectionString("DefaultConnection");
            Log.Information("Final connection string contains USERNAME placeholder: {ContainsUsername}", 
                finalConnectionString.Contains("${USERNAME}"));
                
            Log.Information("Database connection string configured successfully with credentials");
        }
        else
        {
            Log.Error("Failed to configure database connection string with proper credentials");
            Log.Error("Azure Key Vault URI: {KeyVaultUri}", configuration["KeyVault:VaultUri"]);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error configuring connection string with Key Vault values");
    }
}
