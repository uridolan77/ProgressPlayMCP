using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for transactions
/// </summary>
[Authorize]
public class TransactionsController : BaseController
{
    private readonly IProgressPlayApiClient _apiClient;
    private readonly IDateValidator _dateValidator;
    private readonly ILogger<TransactionsController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiClient">API client</param>
    /// <param name="dateValidator">Date validator</param>
    /// <param name="logger">Logger</param>
    public TransactionsController(
        IProgressPlayApiClient apiClient,
        IDateValidator dateValidator,
        ILogger<TransactionsController> logger)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get players' individual transactions details
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of transactions</returns>
    [HttpPost]
    [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactions(TransactionsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting transactions for the period {StartDate} to {EndDate}", request.DateStart, request.DateEnd);

        // Validate dates
        if (!_dateValidator.IsValidDate(request.DateStart) || !_dateValidator.IsValidDate(request.DateEnd))
        {
            return BadRequest("Invalid date format. Use YYYY/MM/DD format.");
        }

        if (!_dateValidator.IsValidDateRange(request.DateStart, request.DateEnd))
        {
            return BadRequest("Invalid date range. Start date must be before or equal to end date.");
        }

        // Validate white labels
        if (request.WhiteLabels == null || !request.WhiteLabels.Any())
        {
            return BadRequest("At least one white label ID must be provided.");
        }

        try
        {
            var result = await _apiClient.GetTransactionsAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transactions: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}