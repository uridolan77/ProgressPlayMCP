using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressPlayMCP.Core.Models.Database;

/// <summary>
/// Represents a game in the database
/// </summary>
public class Game
{
    /// <summary>
    /// Game ID (primary key)
    /// </summary>
    [Key]
    public int? GameID { get; set; }

    /// <summary>
    /// Game name
    /// </summary>
    public string GameName { get; set; }

    /// <summary>
    /// Game provider
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// Sub-provider
    /// </summary>
    public string SubProvider { get; set; }

    /// <summary>
    /// Game type
    /// </summary>
    public string GameType { get; set; }

    /// <summary>
    /// Server game ID
    /// </summary>
    public string ServerGameID { get; set; }

    /// <summary>
    /// Game filters (comma-separated)
    /// </summary>
    public string GameFilters { get; set; }

    /// <summary>
    /// Whether the game is hidden in lobby
    /// </summary>
    public bool? HideInLobby { get; set; }

    /// <summary>
    /// Navigation property to daily action games
    /// </summary>
    public virtual ICollection<DailyActionGame> DailyActionGames { get; set; }

    /// <summary>
    /// Navigation property to big winners
    /// </summary>
    public virtual ICollection<BigWinner> BigWinners { get; set; }
}