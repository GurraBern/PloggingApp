using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;

namespace PloggingAPI.Services;

public class RankingService : IRankingService
{
    private readonly IMongoCollection<UserRanking> _rankingCollection;

    public RankingService(IOptions<PloggingDatabaseSettings> rankingDatabaseSettings)
    {
        var mongoClient = new MongoClient(rankingDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(rankingDatabaseSettings.Value.DatabaseName);
        _rankingCollection = mongoDatabase.GetCollection<UserRanking>(rankingDatabaseSettings.Value.RankingCollectionName);
    }

    public async Task<IEnumerable<UserRanking>> GetRankings()
    {
        var rankings = await _rankingCollection.Find(_ => true).ToListAsync();
        return rankings;
    }
}
