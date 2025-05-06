using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for player summary
/// </summary>
[Authorize]
public class PlayerSummaryController : PermissionFilteredController
{
    private readonly IProgressPlayApiClient _apiClient;
    private readonly IDateValidator _dateValidator;
    private readonly ILogger<PlayerSummaryController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiClient">API client</param>
    /// <param name="dateValidator">Date validator</param>
    /// <param name="userService">User service for permission filtering</param>
    /// <param name="logger">Logger</param>
    public PlayerSummaryController(
        IProgressPlayApiClient apiClient,
        IDateValidator dateValidator,
        IUserService userService,
        ILogger<PlayerSummaryController> logger)
        : base(userService)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get players' summarized financial activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player summaries</returns>
    [HttpPost]
    [ProducesResponseType(typeof(List<PlayerSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPlayerSummary(PlayerSummaryRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting player summary for the period {StartDate} to {EndDate}", request.DateStart, request.DateEnd);

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
            // Apply WhiteLabel permission filtering
            var allowedWhiteLabels = await FilterWhiteLabelIdsAsync(request.WhiteLabels);
            
            if (!allowedWhiteLabels.Any())
            {
                _logger.LogWarning("User doesn't have access to any of the requested WhiteLabels");
                return Forbid("You don't have permission to access the requested WhiteLabels.");
            }

            // Update the request with only the allowed WhiteLabels
            request.WhiteLabels = allowedWhiteLabels;

            // Apply AffiliateID filtering if specified
            if (!string.IsNullOrEmpty(request.AffiliateId))
            {
                // Check if user has access to this affiliate for any of the requested WhiteLabels
                bool hasAffiliateAccess = false;
                foreach (var whiteLabelId in request.WhiteLabels)
                {
                    if (await HasAffiliateAccessAsync(whiteLabelId, request.AffiliateId))
                    {
                        hasAffiliateAccess = true;
                        break;
                    }
                }

                if (!hasAffiliateAccess)
                {
                    _logger.LogWarning("User doesn't have access to the requested Affiliate ID {AffiliateId}", request.AffiliateId);
                    return Forbid("You don't have permission to access the requested Affiliate ID.");
                }
            }

            var result = await _apiClient.GetPlayerSummaryAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting player summary: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}