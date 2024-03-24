﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Services;

namespace PloggingAPI.Repository;

public class LitterbagRepository : ILitterbagRepository
{
    private readonly IMongoCollection<LitterbagPlacement> _litterbagPlacementCollection;

    public LitterbagRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _litterbagPlacementCollection = mongoDatabase.GetCollection<LitterbagPlacement>(settings.Value.LitterbagPlacementCollectionName);
    }

    public async Task DeleteLitterbagPlacement(string litterbagPlacementId)
    {
        await _litterbagPlacementCollection.DeleteOneAsync(litterbagPlacement => litterbagPlacement.Id.Equals(litterbagPlacementId));
    }

    public async Task<IEnumerable<LitterbagPlacement>> GetAllLitterbagPlacements()
    {
        var litterbagPlacements = await _litterbagPlacementCollection.FindAsync(_ => true);

        return litterbagPlacements.ToList();
    }

    public async Task InsertLitterbagPlacement(LitterbagPlacement litterbagPlacement)
    {
        await _litterbagPlacementCollection.InsertOneAsync(litterbagPlacement);
    }
}
