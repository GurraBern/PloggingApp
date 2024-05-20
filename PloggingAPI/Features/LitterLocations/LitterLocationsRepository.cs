using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;

namespace PloggingAPI.Features.LitterLocations;

public class LitterLocationsRepository : ILitterLocationsRepository
{
    private readonly IMongoCollection<LitterLocation> _litterLocationsCollection;

    public LitterLocationsRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _litterLocationsCollection = mongoDatabase.GetCollection<LitterLocation>(settings.Value.LitterLocationsCollectionName);
    }

    public async Task InsertLitterLocations(IEnumerable<LitterLocation> litterLocations)
    {
        if (litterLocations.Any())
            await _litterLocationsCollection.InsertManyAsync(litterLocations);
    }

    //TODO should add radius and datefilter
    public async Task<IEnumerable<LitterLocation>> GetLitterLocations()
    {
        var litterLocations = await _litterLocationsCollection.FindAsync(_ => true);
        return litterLocations.ToList();
    }
}
