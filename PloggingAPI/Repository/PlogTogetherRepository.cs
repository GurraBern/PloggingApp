using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Repository;

public class PlogTogetherRepository : IPlogTogetherRepository
{
	private readonly IMongoCollection<PlogTogether> _plogTogetherCollection;

	public PlogTogetherRepository(IOptions<PloggingDatabaseSettings> settings)
	{
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _plogTogetherCollection = mongoDataBase.GetCollection<PlogTogether>(settings.Value.PlogTogetherCollectionName);
    }

    public async Task AddUserToGroup(string ownerUserId, string userId)
    {
        var userExistsInGroup = await _plogTogetherCollection.Find(a => a.UserIds.Contains(userId) || a.OwnerUserId == userId).FirstOrDefaultAsync();

        if (userExistsInGroup == null)
        {
            var userOwnsGroup = await _plogTogetherCollection.Find(a => a.OwnerUserId == ownerUserId).FirstOrDefaultAsync();

            if (userOwnsGroup == null)
            {
                List<string> userIds = new()
            {
                userId
            };

                var newGroup = new PlogTogether
                {
                    OwnerUserId = ownerUserId,
                    UserIds = userIds
                };

                await _plogTogetherCollection.InsertOneAsync(newGroup);
            }
            else
            {
                if (!userOwnsGroup.UserIds.Contains(userId))
                {
                    userOwnsGroup.UserIds.Add(userId);
                    await _plogTogetherCollection.ReplaceOneAsync(u => u.OwnerUserId == ownerUserId, userOwnsGroup);
                }
            }
        }
        else
        {
            return;
        }
    }

    public async Task DeleteGroup(string ownerUserId)
    {
        await _plogTogetherCollection.DeleteOneAsync(u => u.OwnerUserId == ownerUserId);
    }
}

