using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for player details
/// </summary>
[Authorize]
public class PlayerDetailsController : PermissionFilteredController
{
    private readonly IProgressPlayApiClient _apiClient;
    private readonly IDateValidator _dateValidator;
    private readonly ILogger<PlayerDetailsController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiClient">API client</param>
    /// <param name="dateValidator">Date validator</param>
    /// <param name="userService">User service for permission filtering</param>
    /// <param name="logger">Logger</param>
    public PlayerDetailsController(
        IProgressPlayApiClient apiClient,
        IDateValidator dateValidator,
        IUserService userService,
        ILogger<PlayerDetailsController> logger)
        : base(userService)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get players' life-time details and stats
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(List<PlayerDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPlayerDetails(PlayerDetailsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting player details");

        // Check if at least one date filter is provided
        bool hasRegistrationDate = !string.IsNullOrEmpty(request.RegistrationDateStart) || !string.IsNullOrEmpty(request.RegistrationDateEnd);
        bool hasLastUpdatedDate = !string.IsNullOrEmpty(request.LastUpdatedDateStart) || !string.IsNullOrEmpty(request.LastUpdatedDateEnd);

        if (!hasRegistrationDate && !hasLastUpdatedDate)
        {
            return BadRequest("At least one date filter (registration date or last updated date) must be provided.");
        }

        // Validate registration dates if provided
        if (hasRegistrationDate)
        {
            if (!string.IsNullOrEmpty(request.RegistrationDateStart) && !_dateValidator.IsValidDate(request.RegistrationDateStart))
            {
                return BadRequest("Invalid registration start date format. Use YYYY/MM/DD format.");
            }

            if (!string.IsNullOrEmpty(request.RegistrationDateEnd) && !_dateValidator.IsValidDate(request.RegistrationDateEnd))
            {
                return BadRequest("Invalid registration end date format. Use YYYY/MM/DD format.");
            }

            if (!string.IsNullOrEmpty(request.RegistrationDateStart) && !string.IsNullOrEmpty(request.RegistrationDateEnd) && 
                !_dateValidator.IsValidDateRange(request.RegistrationDateStart, request.RegistrationDateEnd))
            {
                return BadRequest("Invalid registration date range. Start date must be before or equal to end date.");
            }
        }

        // Validate last updated dates if provided
        if (hasLastUpdatedDate)
        {
            if (!string.IsNullOrEmpty(request.LastUpdatedDateStart) && !_dateValidator.IsValidDate(request.LastUpdatedDateStart))
            {
                return BadRequest("Invalid last updated start date format. Use YYYY/MM/DD format.");
            }

            if (!string.IsNullOrEmpty(request.LastUpdatedDateEnd) && !_dateValidator.IsValidDate(request.LastUpdatedDateEnd))
            {
                return BadRequest("Invalid last updated end date format. Use YYYY/MM/DD format.");
            }

            if (!string.IsNullOrEmpty(request.LastUpdatedDateStart) && !string.IsNullOrEmpty(request.LastUpdatedDateEnd) && 
                !_dateValidator.IsValidDateRange(request.LastUpdatedDateStart, request.LastUpdatedDateEnd))
            {
                return BadRequest("Invalid last updated date range. Start date must be before or equal to end date.");
            }
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

            var result = await _apiClient.GetPlayerDetailsAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting player details: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}