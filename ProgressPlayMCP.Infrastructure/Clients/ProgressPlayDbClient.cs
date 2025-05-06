using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProgressPlayMCP.Core.Interfaces;
using ProgressPlayMCP.Core.Models;
using ProgressPlayMCP.Core.Models.Responses;

namespace ProgressPlayMCP.Infrastructure.Clients;

/// <summary>
/// Client for direct database access to ProgressPlayDB in Azure
/// </summary>
public class ProgressPlayDbClient : IProgressPlayDbClient
{
    private readonly string _connectionString;
    private readonly ILogger<ProgressPlayDbClient> _logger;
    private readonly ProgressPlayDbSettings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration">Configuration to get connection string</param>
    /// <param name="logger">Logger</param>
    /// <param name="settings">Database settings</param>
    public ProgressPlayDbClient(
        IConfiguration configuration,
        ILogger<ProgressPlayDbClient> logger,
        IOptions<ProgressPlayDbSettings> settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        
        // Get connection string from configuration or use the one from settings
        _connectionString = configuration.GetConnectionString("ProgressPlayDb") ?? 
                           _settings.ConnectionString ?? 
                           throw new ArgumentException("Connection string for ProgressPlayDB not found");
    }

    /// <summary>
    /// Get daily actions from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="whiteLabelIds">Optional white label IDs to filter by</param>
    /// <returns>List of daily actions</returns>
    public async Task<List<DailyActionResponse>> GetDailyActionsAsync(DateTime startDate, DateTime endDate, List<int> whiteLabelIds = null)
    {
        _logger.LogInformation("Getting daily actions from database for period {StartDate} to {EndDate}", startDate, endDate);

        var result = new List<DailyActionResponse>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                // Build query based on the schema from dailyactionsdb-scheme.sql
                var sql = @"
                    SELECT da.ID, dap.PlayerID, dap.CasinoName, dap.CasinoID AS WhiteLabelID,
                           da.TransactionDate, dap.Currency, dap.Country
                    FROM common.tbl_Daily_actions da
                    JOIN common.tbl_Daily_actions_players dap ON da.PlayerID = dap.PlayerID
                    WHERE da.TransactionDate BETWEEN @StartDate AND @EndDate";

                // Add white label filter if provided
                if (whiteLabelIds != null && whiteLabelIds.Any())
                {
                    sql += " AND dap.CasinoID IN (SELECT value FROM STRING_SPLIT(@WhiteLabelIds, ','))";
                    command.Parameters.Add(new SqlParameter("@WhiteLabelIds", string.Join(",", whiteLabelIds)));
                }

                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate });
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new DailyActionResponse
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PlayerId = reader.GetInt64(reader.GetOrdinal("PlayerID")),
                            CasinoName = reader.GetString(reader.GetOrdinal("CasinoName")),
                            WhiteLabelId = reader.GetInt32(reader.GetOrdinal("WhiteLabelID")),
                            TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                            Currency = reader.GetString(reader.GetOrdinal("Currency")),
                            Country = reader.GetString(reader.GetOrdinal("Country"))
                            // Add other fields based on your DailyActionResponse model
                        });
                    }
                }
            }
        }

        _logger.LogInformation("Retrieved {Count} daily action records from database", result.Count);
        return result;
    }

    /// <summary>
    /// Get player details from the database
    /// </summary>
    /// <param name="playerIds">Optional list of player IDs to filter by</param>
    /// <returns>List of player details</returns>
    public async Task<List<PlayerDetailsResponse>> GetPlayerDetailsAsync(List<long> playerIds = null)
    {
        _logger.LogInformation("Getting player details from database");

        var result = new List<PlayerDetailsResponse>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                var sql = @"
                    SELECT PlayerID, CasinoName, Alias, RegisteredDate, FirstDepositDate, 
                           DateOfBirth, Gender, Country, Currency, Balance, AffiliateID
                    FROM common.tbl_Daily_actions_players";

                // Add player ID filter if provided
                if (playerIds != null && playerIds.Any())
                {
                    sql += " WHERE PlayerID IN (SELECT value FROM STRING_SPLIT(@PlayerIds, ','))";
                    command.Parameters.Add(new SqlParameter("@PlayerIds", string.Join(",", playerIds)));
                }

                command.CommandText = sql;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new PlayerDetailsResponse
                        {
                            PlayerId = reader.GetInt64(reader.GetOrdinal("PlayerID")),
                            CasinoName = reader.GetString(reader.GetOrdinal("CasinoName")),
                            Username = reader.GetString(reader.GetOrdinal("Alias")),
                            RegisteredDate = reader.GetDateTime(reader.GetOrdinal("RegisteredDate")),
                            FirstDepositDate = reader.IsDBNull(reader.GetOrdinal("FirstDepositDate")) ? 
                                null : reader.GetDateTime(reader.GetOrdinal("FirstDepositDate")),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? 
                                null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                            Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? 
                                null : reader.GetString(reader.GetOrdinal("Gender")),
                            Country = reader.GetString(reader.GetOrdinal("Country")),
                            Currency = reader.GetString(reader.GetOrdinal("Currency")),
                            Balance = reader.GetDecimal(reader.GetOrdinal("Balance")),
                            AffiliateId = reader.IsDBNull(reader.GetOrdinal("AffiliateID")) ? 
                                null : reader.GetString(reader.GetOrdinal("AffiliateID"))
                            // Add other fields based on your PlayerDetailsResponse model
                        });
                    }
                }
            }
        }

        _logger.LogInformation("Retrieved {Count} player detail records from database", result.Count);
        return result;
    }

    /// <summary>
    /// Get transactions from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="playerIds">Optional list of player IDs to filter by</param>
    /// <returns>List of transactions</returns>
    public async Task<List<TransactionResponse>> GetTransactionsAsync(DateTime startDate, DateTime endDate, List<long> playerIds = null)
    {
        _logger.LogInformation("Getting transactions from database for period {StartDate} to {EndDate}", startDate, endDate);

        var result = new List<TransactionResponse>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                // Query based on the transaction tables from the schema
                var sql = @"
                    SELECT t.TransactionID, t.PlayerID, t.TransactionDate, t.TransactionType, 
                           t.TransactionAmount, t.TransactionOriginalAmount, t.Status, t.CurrencyCode,
                           p.CasinoName, p.Alias
                    FROM common.tbl_Daily_actions_transactions t
                    JOIN common.tbl_Daily_actions_players p ON t.PlayerID = p.PlayerID
                    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate";

                // Add player ID filter if provided
                if (playerIds != null && playerIds.Any())
                {
                    sql += " AND t.PlayerID IN (SELECT value FROM STRING_SPLIT(@PlayerIds, ','))";
                    command.Parameters.Add(new SqlParameter("@PlayerIds", string.Join(",", playerIds)));
                }

                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate });
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new TransactionResponse
                        {
                            TransactionId = reader.GetInt64(reader.GetOrdinal("TransactionID")),
                            PlayerId = reader.GetInt64(reader.GetOrdinal("PlayerID")),
                            PlayerUsername = reader.GetString(reader.GetOrdinal("Alias")),
                            CasinoName = reader.GetString(reader.GetOrdinal("CasinoName")),
                            TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                            TransactionType = reader.GetString(reader.GetOrdinal("TransactionType")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("TransactionAmount")),
                            OriginalAmount = reader.GetDecimal(reader.GetOrdinal("TransactionOriginalAmount")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            Currency = reader.GetString(reader.GetOrdinal("CurrencyCode"))
                            // Add other fields based on your TransactionResponse model
                        });
                    }
                }
            }
        }

        _logger.LogInformation("Retrieved {Count} transaction records from database", result.Count);
        return result;
    }

    /// <summary>
    /// Get player games from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="playerIds">Optional list of player IDs to filter by</param>
    /// <returns>List of player games</returns>
    public async Task<List<PlayerGameResponse>> GetPlayerGamesAsync(DateTime startDate, DateTime endDate, List<long> playerIds = null)
    {
        _logger.LogInformation("Getting player games from database for period {StartDate} to {EndDate}", startDate, endDate);

        var result = new List<PlayerGameResponse>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                // Query based on the game tables from the schema
                var sql = @"
                    SELECT g.ID, g.PlayerID, g.GameID, g.GameDate, g.RealBetAmount, 
                           g.RealWinAmount, g.BonusBetAmount, g.BonusWinAmount, g.NetGamingRevenue,
                           g.NumberofSessions, ga.GameName, ga.GameType, p.CasinoName, p.Alias
                    FROM common.tbl_Daily_actions_games g
                    JOIN dbo.Games ga ON g.GameID = ga.GameID
                    JOIN common.tbl_Daily_actions_players p ON g.PlayerID = p.PlayerID
                    WHERE g.GameDate BETWEEN @StartDate AND @EndDate";

                // Add player ID filter if provided
                if (playerIds != null && playerIds.Any())
                {
                    sql += " AND g.PlayerID IN (SELECT value FROM STRING_SPLIT(@PlayerIds, ','))";
                    command.Parameters.Add(new SqlParameter("@PlayerIds", string.Join(",", playerIds)));
                }

                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate });
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new PlayerGameResponse
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PlayerId = reader.GetInt64(reader.GetOrdinal("PlayerID")),
                            PlayerUsername = reader.GetString(reader.GetOrdinal("Alias")),
                            CasinoName = reader.GetString(reader.GetOrdinal("CasinoName")),
                            GameId = reader.GetInt32(reader.GetOrdinal("GameID")),
                            GameName = reader.GetString(reader.GetOrdinal("GameName")),
                            GameType = reader.GetString(reader.GetOrdinal("GameType")),
                            GameDate = reader.GetDateTime(reader.GetOrdinal("GameDate")),
                            RealBetAmount = reader.GetDecimal(reader.GetOrdinal("RealBetAmount")),
                            RealWinAmount = reader.GetDecimal(reader.GetOrdinal("RealWinAmount")),
                            BonusBetAmount = reader.GetDecimal(reader.GetOrdinal("BonusBetAmount")),
                            BonusWinAmount = reader.GetDecimal(reader.GetOrdinal("BonusWinAmount")),
                            NetGamingRevenue = reader.GetDecimal(reader.GetOrdinal("NetGamingRevenue")),
                            NumberOfSessions = reader.GetInt32(reader.GetOrdinal("NumberofSessions"))
                            // Add other fields based on your PlayerGameResponse model
                        });
                    }
                }
            }
        }

        _logger.LogInformation("Retrieved {Count} player game records from database", result.Count);
        return result;
    }

    /// <summary>
    /// Get white labels from the database
    /// </summary>
    /// <returns>List of white labels</returns>
    public async Task<List<WhiteLabel>> GetWhiteLabelsAsync()
    {
        _logger.LogInformation("Getting white labels from database");

        var result = new List<WhiteLabel>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT LabelID, LabelName
                    FROM common.tbl_White_labels";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new WhiteLabel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("LabelID")),
                            Name = reader.GetString(reader.GetOrdinal("LabelName"))
                        });
                    }
                }
            }
        }

        _logger.LogInformation("Retrieved {Count} white label records from database", result.Count);
        return result;
    }

    /// <summary>
    /// Get big winners from the database
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="minAmount">Minimum win amount to consider</param>
    /// <returns>List of big winners</returns>
    public async Task<List<BigWinnerResponse>> GetBigWinnersAsync(DateTime startDate, DateTime endDate, decimal minAmount = 1000)
    {
        _logger.LogInformation("Getting big winners from database for period {StartDate} to {EndDate}", startDate, endDate);

        var result = new List<BigWinnerResponse>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT TransactionID, PlayerID, PlayerName, Country, Currency, 
                           GameType, GameName, GameID, Amount, AmountInCurrency, 
                           ActionDate, WhiteLabelID
                    FROM dbo.V_BigWinners
                    WHERE ActionDate BETWEEN @StartDate AND @EndDate
                    AND Amount >= @MinAmount
                    ORDER BY Amount DESC";

                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate });
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate });
                command.Parameters.Add(new SqlParameter("@MinAmount", SqlDbType.Decimal) { Value = minAmount });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new BigWinnerResponse
                        {
                            TransactionId = reader.GetInt64(reader.GetOrdinal("TransactionID")),
                            PlayerId = reader.GetInt64(reader.GetOrdinal("PlayerID")),
                            PlayerName = reader.GetString(reader.GetOrdinal("PlayerName")),
                            Country = reader.GetString(reader.GetOrdinal("Country")),
                            Currency = reader.GetString(reader.GetOrdinal("Currency")),
                            GameType = reader.GetString(reader.GetOrdinal("GameType")),
                            GameName = reader.GetString(reader.GetOrdinal("GameName")),
                            GameId = reader.GetInt32(reader.GetOrdinal("GameID")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            AmountInCurrency = reader.GetString(reader.GetOrdinal("AmountInCurrency")),
                            ActionDate = reader.GetDateTime(reader.GetOrdinal("ActionDate")),
                            WhiteLabelId = reader.GetInt32(reader.GetOrdinal("WhiteLabelID"))
                        });
                    }
                }
            }
        }

        _logger.LogInformation("Retrieved {Count} big winner records from database", result.Count);
        return result;
    }

    /// <summary>
    /// Execute a stored procedure to update daily actions
    /// </summary>
    /// <returns>Number of affected rows</returns>
    public async Task<int> UpdateDailyActionsAsync()
    {
        _logger.LogInformation("Executing stored procedure to update daily actions");

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "dbo.stp_DailyActions";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the stored procedure
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}