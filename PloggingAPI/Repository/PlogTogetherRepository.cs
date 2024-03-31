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
        var userOwnsGroup = await _plogTogetherCollection.Find(u => u.OwnerUserId == ownerUserId).FirstOrDefaultAsync();
        if (userOwnsGroup == null)
        {
            var ownerMemberInGroup = await _plogTogetherCollection.Find(u => u.UserIds.Contains(ownerUserId)).FirstOrDefaultAsync();
            if (ownerMemberInGroup == null)
            {
                var userMemberInGroup = await _plogTogetherCollection.Find(u => u.UserIds.Contains(userId)).FirstOrDefaultAsync();
                if (userMemberInGroup == null)
                {
                    List<string> userIds = new()
                    {
                         ownerUserId,
                         userId
                    };

                    var newGroup = new PlogTogether
                    {
                        OwnerUserId = ownerUserId,
                        UserIds = userIds
                    };

                    await _plogTogetherCollection.InsertOneAsync(newGroup);
                }
                else // user exists in another group
                {
                    return;
                }
            }
            else // owner exists in another group
            {
                return;
            }
        }
        else //owner har redan en grupp
        {
            var userMemberInGroup = await _plogTogetherCollection.Find(u => u.UserIds.Contains(userId)).FirstOrDefaultAsync();
            if (userMemberInGroup == null)
            {
                userOwnsGroup.UserIds.Add(userId);
                await _plogTogetherCollection.ReplaceOneAsync(u => u.OwnerUserId == ownerUserId, userOwnsGroup);
            }
            else // user exists in another group
            {
                return;
            }
        }
    }

    public async Task DeleteGroup(string ownerUserId)
    {
        await _plogTogetherCollection.DeleteOneAsync(u => u.OwnerUserId == ownerUserId);
    }

    public async Task<PlogTogether> GetPlogTogether(string userId)
    {
        var plogTogether = await _plogTogetherCollection.Find(u => u.UserIds.Contains(userId)).FirstOrDefaultAsync();

        return plogTogether;
    }

    public async Task RemoveUserFromGroup(string ownerUserId, string userId)
    {
        var plogGroup = await _plogTogetherCollection.Find(a => a.OwnerUserId == ownerUserId).FirstOrDefaultAsync();

        plogGroup.UserIds.Remove(userId);

        if (plogGroup.UserIds.Count == 0)
        {
            await DeleteGroup(ownerUserId);
        }
        else
        {
            await _plogTogetherCollection.ReplaceOneAsync(u => u.OwnerUserId == ownerUserId, plogGroup);
        }
    }
}
