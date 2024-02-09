using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Repository;

public class RankingRepository : IRankingRepository
{
    private readonly IMongoCollection<UserRanking> _rankingCollection;

    public RankingRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _rankingCollection = mongoDatabase.GetCollection<UserRanking>(settings.Value.RankingCollectionName);
    }

    public async Task<IEnumerable<UserRanking>> GetAllUserRankings()
    {
        var rankings = await _rankingCollection
            .Find(_ => true)
            .ToListAsync();

        return rankings;
    }

    public async Task<IEnumerable<UserRanking>> GetUserRankings(int pageNumber, int pageSize)
    {
        var rankings = await _rankingCollection
            .Find(_ => true)
            .Skip(pageSize * (pageNumber - 1))
            .Limit(pageSize)
            .ToListAsync();

        return rankings;
    }

    public async Task UpdateUserRankings(IEnumerable<UserRanking> userRankings)
    {
        //TODO update all users rankings 
    }
}
