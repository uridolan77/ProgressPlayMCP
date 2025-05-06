using ProgressPlayMCP.Core.Models;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Interface for direct database access to ProgressPlayDB
/// </summary>
public interface IProgressPlayDbClient
{
    /// <summary>
    /// Get daily actions from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="whiteLabelIds">Optional white label IDs to filter by</param>
    /// <returns>List of daily actions</returns>
    Task<List<DailyActionResponse>> GetDailyActionsAsync(DateTime startDate, DateTime endDate, List<int> whiteLabelIds = null);

    /// <summary>
    /// Get player details from the database
    /// </summary>
    /// <param name="playerIds">Optional list of player IDs to filter by</param>
    /// <returns>List of player details</returns>
    Task<List<PlayerDetailsResponse>> GetPlayerDetailsAsync(List<long> playerIds = null);

    /// <summary>
    /// Get transactions from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="playerIds">Optional list of player IDs to filter by</param>
    /// <returns>List of transactions</returns>
    Task<List<TransactionResponse>> GetTransactionsAsync(DateTime startDate, DateTime endDate, List<long> playerIds = null);

    /// <summary>
    /// Get player games from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="playerIds">Optional list of player IDs to filter by</param>
    /// <returns>List of player games</returns>
    Task<List<PlayerGameResponse>> GetPlayerGamesAsync(DateTime startDate, DateTime endDate, List<long> playerIds = null);

    /// <summary>
    /// Get white labels from the database
    /// </summary>
    /// <returns>List of white labels</returns>
    Task<List<WhiteLabel>> GetWhiteLabelsAsync();

    /// <summary>
    /// Get big winners from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="minAmount">Minimum win amount to consider</param>
    /// <returns>List of big winners</returns>
    Task<List<BigWinnerResponse>> GetBigWinnersAsync(DateTime startDate, DateTime endDate, decimal minAmount = 1000);

    /// <summary>
    /// Execute a stored procedure to update daily actions
    /// </summary>
    /// <returns>Number of affected rows</returns>
    Task<int> UpdateDailyActionsAsync();
}