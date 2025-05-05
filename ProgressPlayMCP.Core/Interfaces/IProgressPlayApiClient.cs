using ProgressPlayMCP.Core.Models.Requests;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.Core.Interfaces;

/// <summary>
/// Interface for the ProgressPlay API client
/// </summary>
public interface IProgressPlayApiClient
{
    /// <summary>
    /// Get players' summarized daily financial activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of daily actions</returns>
    Task<List<DailyActionResponse>> GetDailyActionsAsync(DailyActionsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get players' summarized financial activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player summaries</returns>
    Task<List<PlayerSummaryResponse>> GetPlayerSummaryAsync(PlayerSummaryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get players' life-time details and stats
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player details</returns>
    Task<List<PlayerDetailsResponse>> GetPlayerDetailsAsync(PlayerDetailsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get players' individual transactions details
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of transactions</returns>
    Task<List<TransactionResponse>> GetTransactionsAsync(TransactionsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get players' summarized play activity
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of player games</returns>
    Task<List<PlayerGameResponse>> GetPlayerGamesAsync(PlayerGamesRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get data in Income Access format
    /// </summary>
    /// <param name="request">Request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of income access data</returns>
    Task<List<IncomeAccessResponse>> GetIncomeAccessAsync(IncomeAccessRequest request, CancellationToken cancellationToken = default);
}