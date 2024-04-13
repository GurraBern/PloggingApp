using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Repository;

public class UserEventRepository : IUserEventRepository
{
    private readonly IMongoCollection<UserEvent> _userEventCollection;

    public UserEventRepository(IOptions<PloggingDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _userEventCollection = mongoDatabase.GetCollection<UserEvent>(settings.Value.UserEventsCollectionName);
    }

    public async Task CreateEvent(UserEvent userEvent)
    {
        await _userEventCollection.InsertOneAsync(userEvent);
    }

    public async Task<IEnumerable<UserEvent>> GetEvents()
    {
        var userEvents = await _userEventCollection.FindAsync(_ => true);

        return userEvents.ToList();
    }
}
