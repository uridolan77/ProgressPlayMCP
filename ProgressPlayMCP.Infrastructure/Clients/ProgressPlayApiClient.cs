using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models;
using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.Infrastructure.Clients;

/// <summary>
/// Implementation of the ProgressPlay API client
/// </summary>
public class ProgressPlayApiClient : IProgressPlayApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProgressPlayApiClient> _logger;
    private readonly ProgressPlayApiSettings _settings;
    private readonly IApiExceptionHandler _exceptionHandler;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpClient">HTTP client</param>
    /// <param name="logger">Logger</param>
    /// <param name="settings">API settings</param>
    /// <param name="exceptionHandler">Exception handler</param>
    public ProgressPlayApiClient(
        HttpClient httpClient,
        ILogger<ProgressPlayApiClient> logger,
        IOptions<ProgressPlayApiSettings> settings,
        IApiExceptionHandler exceptionHandler)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        // Configure the HTTP client with base URI and authentication
        ConfigureHttpClient();
    }

    /// <summary>
    /// Get players' summarized daily financial activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of daily actions</returns>
    public async Task<List<DailyActionResponse>> GetDailyActionsAsync(DailyActionsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting daily actions for period {StartDate} to {EndDate}", request.DateStart, request.DateEnd);
            
            // Set default currency if not provided
            if (string.IsNullOrEmpty(request.TargetCurrency))
            {
                request.TargetCurrency = _settings.DefaultCurrency;
            }

            var response = await SendRequestAsync<List<DailyActionResponse>>("dailyactions", request, cancellationToken);
            
            _logger.LogInformation("Retrieved {Count} daily action records", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting daily actions: {Message}", ex.Message);
            throw _exceptionHandler.HandleException(ex);
        }
    }

    /// <summary>
    /// Get players' summarized financial activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player summaries</returns>
    public async Task<List<PlayerSummaryResponse>> GetPlayerSummaryAsync(PlayerSummaryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting player summary for period {StartDate} to {EndDate}", request.DateStart, request.DateEnd);
            
            // Set default currency if not provided
            if (string.IsNullOrEmpty(request.TargetCurrency))
            {
                request.TargetCurrency = _settings.DefaultCurrency;
            }

            var response = await SendRequestAsync<List<PlayerSummaryResponse>>("playersummary", request, cancellationToken);
            
            _logger.LogInformation("Retrieved {Count} player summary records", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting player summary: {Message}", ex.Message);
            throw _exceptionHandler.HandleException(ex);
        }
    }

    /// <summary>
    /// Get players' life-time details and stats
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player details</returns>
    public async Task<List<PlayerDetailsResponse>> GetPlayerDetailsAsync(PlayerDetailsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting player details");
            
            var response = await SendRequestAsync<List<PlayerDetailsResponse>>("playerdetails", request, cancellationToken);
            
            _logger.LogInformation("Retrieved {Count} player details records", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting player details: {Message}", ex.Message);
            throw _exceptionHandler.HandleException(ex);
        }
    }

    /// <summary>
    /// Get players' individual transactions details
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of transactions</returns>
    public async Task<List<TransactionResponse>> GetTransactionsAsync(TransactionsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting transactions for period {StartDate} to {EndDate}", request.DateStart, request.DateEnd);
            
            var response = await SendRequestAsync<List<TransactionResponse>>("transactions", request, cancellationToken);
            
            _logger.LogInformation("Retrieved {Count} transaction records", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transactions: {Message}", ex.Message);
            throw _exceptionHandler.HandleException(ex);
        }
    }

    /// <summary>
    /// Get players' summarized play activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player games</returns>
    public async Task<List<PlayerGameResponse>> GetPlayerGamesAsync(PlayerGamesRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting player games for period {StartDate} to {EndDate}", request.DateStart, request.DateEnd);
            
            var response = await SendRequestAsync<List<PlayerGameResponse>>("playergames", request, cancellationToken);
            
            _logger.LogInformation("Retrieved {Count} player game records", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting player games: {Message}", ex.Message);
            throw _exceptionHandler.HandleException(ex);
        }
    }

    /// <summary>
    /// Get data in Income Access format
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of income access data</returns>
    public async Task<List<IncomeAccessResponse>> GetIncomeAccessAsync(IncomeAccessRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting income access data for period {StartDate} to {EndDate}", request.StartDate, request.EndDate);
            
            var response = await SendRequestAsync<List<IncomeAccessResponse>>("incomeaccess", request, cancellationToken, true);
            
            _logger.LogInformation("Retrieved {Count} income access records", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting income access data: {Message}", ex.Message);
            throw _exceptionHandler.HandleException(ex);
        }
    }

    /// <summary>
    /// Configure the HTTP client with base address and authentication
    /// </summary>
    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        
        // Add basic authentication
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        
        // Add content type header
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Send a request to the API
    /// </summary>
    /// <typeparam name="TResponse">Type of the response</typeparam>
    /// <param name="endpoint">API endpoint</param>
    /// <param name="request">Request object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="useDataPath">Whether to use the data path in the URL</param>
    /// <returns>Response object</returns>
    private async Task<TResponse> SendRequestAsync<TResponse>(string endpoint, object request, CancellationToken cancellationToken, bool useDataPath = false)
    {
        // Prepare URL (different path for Income Access)
        var url = useDataPath ? $"data/{endpoint}" : endpoint;
        
        // Serialize request
        var content = new StringContent(
            JsonSerializer.Serialize(request, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        // Send request
        var response = await _httpClient.PostAsync(url, content, cancellationToken);

        // Check if request was successful
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"API request failed with status code {response.StatusCode}: {errorContent}");
        }

        // Deserialize response
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
            return result ?? throw new JsonException("Response content is null");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing response: {Content}", responseContent);
            throw;
        }
    }
}