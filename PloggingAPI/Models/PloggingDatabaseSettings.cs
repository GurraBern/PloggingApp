namespace PloggingAPI.Models;

public class PloggingDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string RankingCollectionName { get; set; } = null!;
    public string PloggingSessionsCollectionName { get; set; } = null!;
    public string StreakCollectionName { get; set; } = null!;
    public string LitterLocationsCollectionName { get; set; } = null!;
    public string PlogTogetherCollectionName { get; set; } = null!;
}
