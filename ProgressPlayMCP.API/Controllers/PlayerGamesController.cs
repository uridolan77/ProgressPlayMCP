using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for player games
/// </summary>
[Authorize]
public class PlayerGamesController : BaseController
{
    private readonly IProgressPlayApiClient _apiClient;
    private readonly IDateValidator _dateValidator;
    private readonly ILogger<PlayerGamesController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiClient">API client</param>
    /// <param name="dateValidator">Date validator</param>
    /// <param name="logger">Logger</param>
    public PlayerGamesController(
        IProgressPlayApiClient apiClient,
        IDateValidator dateValidator,
        ILogger<PlayerGamesController> logger)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get players' summarized play activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player games</returns>
    [HttpPost]
    [ProducesResponseType(typeof(List<PlayerGameResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPlayerGames(PlayerGamesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting player games for the period {StartDate} to {EndDate}", request.DateStart, request.DateEnd);

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
            var result = await _apiClient.GetPlayerGamesAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting player games: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}