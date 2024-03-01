using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Repository;

public class LitterLocationsRepository : ILitterLocationsRepository
{
    private readonly IMongoCollection<GeoLocation> _litterLocationsCollection;

    public LitterLocationsRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _litterLocationsCollection = mongoDatabase.GetCollection<GeoLocation>(settings.Value.LitterLocationsCollectionName);
    }

    public async Task InsertLitterLocations(IEnumerable<GeoLocation> litterLocations)
    {
        await _litterLocationsCollection.InsertManyAsync(litterLocations);
    }
}
