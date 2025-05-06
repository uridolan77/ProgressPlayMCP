namespace ProgressPlayMCP.Core.Models;

/// <summary>
/// Settings for the ProgressPlayDB database client
/// </summary>
public class ProgressPlayDbSettings
{
    /// <summary>
    /// Connection string for the database
    /// </summary>
    public string ConnectionString { get; set; } = "Server=tcp:progressplay-server.database.windows.net,1433;Initial Catalog=ProgressPlayDB;Persist Security Info=False;User ID=pp-sa;Password=RDlS****Y5Y;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
}