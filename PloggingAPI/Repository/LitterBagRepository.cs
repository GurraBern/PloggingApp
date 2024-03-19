using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Services;

namespace PloggingAPI.Repository;

public class LitterBagRepository : ILitterBagRepository
{
    private readonly IMongoCollection<LitterBagPlacement> _litterBagPlacementCollection;

    public LitterBagRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _litterBagPlacementCollection = mongoDatabase.GetCollection<LitterBagPlacement>(settings.Value.LitterBagPlacementCollectionName);
    }

    public async Task<IEnumerable<LitterBagPlacement>> GetAllLitterBagPlacements()
    {
        var litterBagPlacements = await _litterBagPlacementCollection.FindAsync(_ => true);

        return litterBagPlacements.ToList();
    }

    public async Task InsertLitterBagPlacement(LitterBagPlacement litterBagPlacement)
    {
        await _litterBagPlacementCollection.InsertOneAsync(litterBagPlacement);
    }
}
