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


    // try to add user to your group
    //      if group does not exist: create a group and add the user
    //      if group does exist add the user to the existing list of users

    public async Task AddUserToGroup(string ownerUserId, string addUserId)
    {
        var result = await _plogTogetherCollection.Find(result => result.OwnerUserId == ownerUserId).FirstOrDefaultAsync();

        if (result == null)
        {
            List<string> userIds = new()
            {
                addUserId
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
            result.UserIds.Add(addUserId);
            await _plogTogetherCollection.ReplaceOneAsync(u => u.OwnerUserId == ownerUserId, result);
        }
    }


    // if finished adding the plogging session to group members
    //      delete the document from the collection

    public async Task DeleteGroup(string ownerUserId)
    {
        await _plogTogetherCollection.DeleteOneAsync(u => u.OwnerUserId == ownerUserId);
    }
}

