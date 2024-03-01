using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Factories;
using PloggingAPI.Models;
using PloggingAPI.Models.Queries;
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

    public async Task InsertPloggingSession(PloggingSession ploggingSession)
    {
        await _ploggingSessionCollection.InsertOneAsync(ploggingSession);
    }

    public async Task<IEnumerable<PloggingSession>> GetPloggingSessions(string userId, DateTime startDate, DateTime endDate)
    {
        var matchFilter = Builders<PloggingSession>.Filter.Eq(f => f.UserId, userId) &
            Builders<PloggingSession>.Filter.Gte(f => f.StartDate, startDate) &
            Builders<PloggingSession>.Filter.Lte(f => f.StartDate, endDate);

        var sortDefinition = Builders<PloggingSession>.Sort.Descending(x => x.StartDate);

        var pipeline = new EmptyPipelineDefinition<PloggingSession>()
            .Match(matchFilter)
            .Sort(sortDefinition);

        var sessions = await _ploggingSessionCollection.Aggregate(pipeline).ToListAsync();

        return sessions;
    }

    public async Task<IEnumerable<PloggingSession>> GetSessionSummaries(SessionSummaryQuery query)
    {
        var sortDefinition = SortDefinitionFactory.CreateSortDefinition(query);

        var matchFilter = Builders<PloggingSession>.Filter.Gte(f => f.StartDate, query.StartDate) &
            Builders<PloggingSession>.Filter.Lte(f => f.EndDate, query.EndDate);

        var pipeline = new EmptyPipelineDefinition<PloggingSession>()
            .Match(matchFilter)
            .Group(f => f.UserId,
               g => new PloggingSession
               {
                   UserId = g.Key,
                   DisplayName = g.First().DisplayName,
                   PloggingData = new()
                   {
                       //ScrapCount = g.Sum(f => f.PloggingData.ScrapCount),
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
