using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PloggingAPI.Models;
using PlogPal.Domain.Models;

namespace PloggingAPI.Features.Plogtogether;

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
        var ownerMemberInGroup = await _plogTogetherCollection.Find(u => u.UserIds.Contains(ownerUserId)).FirstOrDefaultAsync();
        var userMemberInGroup = await _plogTogetherCollection.Find(u => u.UserIds.Contains(userId)).FirstOrDefaultAsync();

        if (userOwnsGroup != null)
        {
            if (userMemberInGroup == null)
            {
                userOwnsGroup.UserIds.Add(userId);
                await _plogTogetherCollection.ReplaceOneAsync(u => u.OwnerUserId == ownerUserId, userOwnsGroup);
            }
            return;
        }

        if (ownerMemberInGroup != null || userMemberInGroup != null)
            return;

        var newGroup = new PlogTogether
        {
            OwnerUserId = ownerUserId,
            UserIds = new List<string> { ownerUserId, userId }
        };

        await _plogTogetherCollection.InsertOneAsync(newGroup);
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

    public async Task LeaveGroup(string userId)
    {
        var plogGroup = await _plogTogetherCollection.Find(u => u.UserIds.Contains(userId)).FirstOrDefaultAsync();
        var groupOwnerId = plogGroup.OwnerUserId;

        if (groupOwnerId != userId)
        {
            plogGroup.UserIds.Remove(userId);

            if (plogGroup.UserIds.Count == 1)
            {
                await DeleteGroup(groupOwnerId);
            }
            else
            {
                await _plogTogetherCollection.ReplaceOneAsync(u => u.OwnerUserId == groupOwnerId, plogGroup);
            }
        }
    }

    /*public async Task RemoveUserFromGroup(string ownerUserId, string userId)
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
    }*/
}
