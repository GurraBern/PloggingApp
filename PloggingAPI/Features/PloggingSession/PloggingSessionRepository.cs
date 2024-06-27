using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PlogPal.Domain.Models;
using PloggingAPI.Models;

namespace PloggingAPI.Features.PloggingSession;

public class PloggingSessionRepository : IPloggingSessionRepository
{
    private readonly IMongoCollection<PlogSession> _ploggingSessionCollection;

    public PloggingSessionRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _ploggingSessionCollection = mongoDatabase.GetCollection<PlogSession>(settings.Value.PloggingSessionsCollectionName);
    }

    public async Task InsertPloggingSession(PlogSession ploggingSession)
    {
        await _ploggingSessionCollection.InsertOneAsync(ploggingSession);
    }

    public async Task<IEnumerable<PlogSession>> GetPloggingSessions(string userId, DateTime startDate, DateTime endDate)
    {
        var matchFilter = Builders<PlogSession>.Filter.Eq(f => f.UserId, userId) &
            Builders<PlogSession>.Filter.Gte(f => f.StartDate, startDate) &
            Builders<PlogSession>.Filter.Lte(f => f.EndDate, endDate);

        var sortDefinition = Builders<PlogSession>.Sort.Descending(x => x.StartDate);

        var pipeline = new EmptyPipelineDefinition<PlogSession>()
            .Match(matchFilter)
            .Sort(sortDefinition);

        var sessions = await _ploggingSessionCollection.Aggregate(pipeline).ToListAsync();

        return sessions;
    }

    public async Task<IEnumerable<PlogSession>> GetSessionSummaries(SessionSummaryQuery query)
    {
        var sortDefinition = SortDefinitionFactory.CreateSortDefinition(query);

        var matchFilter = Builders<PlogSession>.Filter.Gte(f => f.StartDate, query.StartDate) &
            Builders<PlogSession>.Filter.Lte(f => f.EndDate, query.EndDate);

        var pipeline = new EmptyPipelineDefinition<PlogSession>()
            .Match(matchFilter)
            .Group(f => f.UserId,
               g => new PlogSession
               {
                   UserId = g.Key,
                   DisplayName = g.First().DisplayName,
                   PloggingData = new()
                   {
                       Weight = g.Sum(f => f.PloggingData.Weight),
                       Distance = g.Sum(f => f.PloggingData.Distance),
                       Steps = g.Sum(f => f.PloggingData.Steps)
                   },
                   StartDate = query.StartDate,
                   EndDate = query.EndDate
               })
            .Sort(sortDefinition);

        var sessions = await _ploggingSessionCollection.Aggregate(pipeline).ToListAsync();

        return sessions;
    }
}
