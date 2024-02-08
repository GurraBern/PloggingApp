using Microsoft.Extensions.Options;
using MongoDB.Bson;
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

    //TODO Get all rankings over a specific TIME PERIOD
    public async Task<IEnumerable<UserRanking>> GetRankings()
    {
        var pipeline = new List<BsonDocument>
        {
            new() {
                {"$group", new BsonDocument
                    {
                        {"_id", "$DisplayName"},
                        {"DisplayName", new BsonDocument { { "$first", "$DisplayName" } } },
                        {"ScrapCount", new BsonDocument {{"$sum", "$ScrapCount"}}}
                    }
                }
            },
            new() {
                {"$sort", new BsonDocument {{"TotalScrapCount", -1}}}
            }
        };

        var rankings = await _rankingCollection.Aggregate<UserRanking>(pipeline).ToListAsync();

        //TODO Is it possible to do in pipeline above?
        var rankingsList = rankings.Select((ranking, index) =>
        {
            ranking.Rank = index + 1;
            return ranking;
        }).ToList();

        return rankingsList;
    }
}
