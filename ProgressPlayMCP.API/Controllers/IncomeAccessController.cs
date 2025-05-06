using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Controller for income access data
/// </summary>
[Authorize]
public class IncomeAccessController : PermissionFilteredController
{
    private readonly IProgressPlayApiClient _apiClient;
    private readonly IDateValidator _dateValidator;
    private readonly ILogger<IncomeAccessController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiClient">API client</param>
    /// <param name="dateValidator">Date validator</param>
    /// <param name="userService">User service for permission filtering</param>
    /// <param name="logger">Logger</param>
    public IncomeAccessController(
        IProgressPlayApiClient apiClient,
        IDateValidator dateValidator,
        IUserService userService,
        ILogger<IncomeAccessController> logger)
        : base(userService)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get data in Income Access format
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of income access data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(List<IncomeAccessResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIncomeAccess(IncomeAccessRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting income access data for the period {StartDate} to {EndDate}", request.StartDate, request.EndDate);

        // Validate dates
        if (!_dateValidator.IsValidDate(request.StartDate) || !_dateValidator.IsValidDate(request.EndDate))
        {
            return BadRequest("Invalid date format. Use YYYY/MM/DD format.");
        }

        if (!_dateValidator.IsValidDateRange(request.StartDate, request.EndDate))
        {
            return BadRequest("Invalid date range. Start date must be before or equal to end date.");
        }

        // Validate white label ID
        if (request.WhitelabelId <= 0)
        {
            return BadRequest("A valid white label ID must be provided.");
        }

        // Validate currency
        if (string.IsNullOrEmpty(request.TargetCurrency))
        {
            return BadRequest("A target currency must be provided.");
        }

        try
        {
            // Check if user has access to the requested WhiteLabel
            if (!await HasWhiteLabelAccessAsync(request.WhitelabelId))
            {
                _logger.LogWarning("User doesn't have access to the requested WhiteLabel {WhitelabelId}", request.WhitelabelId);
                return Forbid("You don't have permission to access the requested WhiteLabel.");
            }

            // Apply AffiliateID filtering if specified
            if (!string.IsNullOrEmpty(request.AffiliateId) && !await HasAffiliateAccessAsync(request.WhitelabelId, request.AffiliateId))
            {
                _logger.LogWarning("User doesn't have access to the requested Affiliate ID {AffiliateId} for WhiteLabel {WhitelabelId}", 
                    request.AffiliateId, request.WhitelabelId);
                return Forbid("You don't have permission to access the requested Affiliate ID.");
            }

            var result = await _apiClient.GetIncomeAccessAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting income access data: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}