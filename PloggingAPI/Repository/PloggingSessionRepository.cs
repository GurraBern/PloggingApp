using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Repository;

public class PloggingSessionRepository : IPloggingSessionRepository
{
    private readonly IMongoCollection<PloggingSession> _ploggingSessionCollection;

    public PloggingSessionRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _ploggingSessionCollection = mongoDatabase.GetCollection<PloggingSession>(settings.Value.PloggingSessionsCollectionName);
    }

    public async Task<IEnumerable<PloggingSession>> GetSessions()
    {
        var sessions = await _ploggingSessionCollection.Find(_ => true).ToListAsync();
        return sessions;
    }

    public async Task<IEnumerable<PloggingSession>> GetSessionSummaries()
    {
        var pipeline = new List<BsonDocument>
        {
            new() {
                {"$group", new BsonDocument
                    {
                        {"_id", "$UserId"},
                        {"UserId", new BsonDocument {{"$first", "$UserId"}}},
                        {"SessionDate", new BsonDocument {{"$last", "$SessionDate"}}},
                        {"ScrapCount", new BsonDocument {{"$sum", "$ScrapCount"}}},
                        {"Distance", new BsonDocument {{"$sum", "$Distance"}}},
                        {"Steps", new BsonDocument {{"$sum", "$Steps"}}},
                    }
                }
            },
            new() {
                {"$sort", new BsonDocument {{"TotalScrapCount", -1}}}
            }
        };

        var sessions = await _ploggingSessionCollection.Aggregate<PloggingSession>(pipeline).ToListAsync();

        return sessions;
    }
}
