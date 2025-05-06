using Microsoft.AspNetCore.Mvc;
using ProgressPlayMCP.Core.Interfaces;
using System.Security.Claims;

namespace ProgressPlayMCP.API.Controllers;

/// <summary>
/// Base controller with permission handling utilities
/// </summary>
public abstract class PermissionFilteredController : BaseController
{
    private readonly IUserService _userService;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userService">User service</param>
    protected PermissionFilteredController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    /// <summary>
    /// Get the current user's ID from claims
    /// </summary>
    /// <returns>User ID or null if not found</returns>
    protected int? GetCurrentUserId()
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(nameIdentifier, out var userId) ? userId : null;
    }

    /// <summary>
    /// Get the current user's allowed WhiteLabel IDs
    /// </summary>
    /// <returns>List of allowed WhiteLabel IDs</returns>
    protected async Task<List<int>> GetAllowedWhiteLabelIdsAsync()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            Console.WriteLine("GetCurrentUserId returned null - no user ID found in claims");
            return new List<int>();
        }

        // Even more detailed logging to understand role checking
        var username = User.Identity?.Name;
        var allClaims = string.Join(", ", User.Claims.Select(c => $"{c.Type}: {c.Value}"));
        var roleClaims = string.Join(", ", User.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role").Select(c => $"{c.Type}: {c.Value}"));
        
        Console.WriteLine($"USER CLAIMS DEBUG - Username: {username}, UserID: {userId}");
        Console.WriteLine($"ALL CLAIMS: {allClaims}");
        Console.WriteLine($"ROLE CLAIMS: {roleClaims}");
        Console.WriteLine($"Is Admin (ClaimTypes.Role): {User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin")}");
        Console.WriteLine($"Is Admin (role): {User.Claims.Any(c => c.Type == "role" && c.Value == "Admin")}");
        Console.WriteLine($"User.IsInRole('Admin'): {User.IsInRole("Admin")}");
        Console.WriteLine($"ClaimsPrincipal Identity Type: {User.Identity?.GetType().FullName}");
        Console.WriteLine($"Authentication Type: {User.Identity?.AuthenticationType}");
        
        // Direct check for "role" claim with value "Admin" 
        var isAdminViaDirectCheck = User.Claims.Any(c => c.Type == "role" && c.Value == "Admin");
        
        // Check if user is an admin (modified to handle both checks)
        if (User.IsInRole("Admin") || isAdminViaDirectCheck)
        {
            Console.WriteLine($"USER IS ADMIN (IsInRole: {User.IsInRole("Admin")}, DirectCheck: {isAdminViaDirectCheck}) - Returning all WhiteLabels");
            return await GetAllWhiteLabelsAsync();
        }

        Console.WriteLine("USER IS NOT ADMIN - Returning filtered WhiteLabels");
        return await _userService.GetUserWhiteLabelsAsync(userId.Value);
    }

    /// <summary>
    /// Get all WhiteLabel IDs in the system
    /// </summary>
    /// <returns>List of all WhiteLabel IDs</returns>
    protected async Task<List<int>> GetAllWhiteLabelsAsync()
    {
        // The hardcoded values don't include the WhiteLabel IDs 1 and 2 that you're requesting
        // Let's modify this to include those IDs, plus logging to better understand what's happening
        Console.WriteLine("GetAllWhiteLabelsAsync called - returning all WhiteLabel IDs");
        
        // Add IDs 1 and 2 to the list of all WhiteLabels
        var allWhiteLabels = new List<int> { 1, 2, 276, 277, 278, 279 };
        
        Console.WriteLine($"All WhiteLabels: {string.Join(", ", allWhiteLabels)}");
        return allWhiteLabels;
    }

    /// <summary>
    /// Get the current user's allowed Affiliate IDs for a specific WhiteLabel
    /// </summary>
    /// <param name="whiteLabelId">WhiteLabel ID</param>
    /// <returns>List of allowed Affiliate IDs</returns>
    protected async Task<List<string>> GetAllowedAffiliateIdsAsync(int whiteLabelId)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return new List<string>();
        }

        // Check if user is an admin (admins can access everything)
        if (User.IsInRole("Admin"))
        {
            // Return all affiliates for this WhiteLabel
            // In a real application, you'd fetch these from a database
            return new List<string> { "all" }; // Special value indicating all affiliates
        }

        // Get user's specific affiliate permissions for this WhiteLabel
        return await _userService.GetUserAffiliatesAsync(userId.Value, whiteLabelId);
    }

    /// <summary>
    /// Filter a request to only include WhiteLabels the user has access to
    /// </summary>
    /// <param name="requestedWhiteLabelIds">Requested WhiteLabel IDs</param>
    /// <returns>Filtered WhiteLabel IDs</returns>
    protected async Task<List<int>> FilterWhiteLabelIdsAsync(List<int> requestedWhiteLabelIds)
    {
        var allowedIds = await GetAllowedWhiteLabelIdsAsync();
        
        // If the user requested specific IDs, filter them by allowed IDs
        if (requestedWhiteLabelIds != null && requestedWhiteLabelIds.Any())
        {
            return requestedWhiteLabelIds.Where(id => allowedIds.Contains(id)).ToList();
        }
        
        // If no specific IDs were requested, return all allowed IDs
        return allowedIds;
    }

    /// <summary>
    /// Check if a user has access to a specific WhiteLabel
    /// </summary>
    /// <param name="whiteLabelId">WhiteLabel ID to check</param>
    /// <returns>True if the user has access, false otherwise</returns>
    protected async Task<bool> HasWhiteLabelAccessAsync(int whiteLabelId)
    {
        var allowedIds = await GetAllowedWhiteLabelIdsAsync();
        return allowedIds.Contains(whiteLabelId);
    }

    /// <summary>
    /// Check if a user has access to a specific affiliate in a WhiteLabel
    /// </summary>
    /// <param name="whiteLabelId">WhiteLabel ID</param>
    /// <param name="affiliateId">Affiliate ID</param>
    /// <returns>True if the user has access, false otherwise</returns>
    protected async Task<bool> HasAffiliateAccessAsync(int whiteLabelId, string affiliateId)
    {
        // First check if the user has access to the WhiteLabel
        if (!await HasWhiteLabelAccessAsync(whiteLabelId))
        {
            return false;
        }

        // If the user is an admin, they have access to all affiliates
        if (User.IsInRole("Admin"))
        {
            return true;
        }

        // Get the user's allowed affiliates for this WhiteLabel
        var allowedAffiliates = await GetAllowedAffiliateIdsAsync(whiteLabelId);
        
        // Special case: if the list contains "all", the user has access to all affiliates
        if (allowedAffiliates.Contains("all"))
        {
            return true;
        }

        // Check if the specific affiliate is in the allowed list
        return allowedAffiliates.Contains(affiliateId);
    }
}