using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Repository;

public class UserInfoRepository : IUserInfoRepository
{
    private readonly IMongoCollection<UserInfo> _userInfoCollection;

    public UserInfoRepository(IOptions<PloggingDatabaseSettings> settings)
	{
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _userInfoCollection = mongoDataBase.GetCollection<UserInfo>(settings.Value.UserInfoCollectionName);
    }

    public async Task CreateUser(UserInfo user)
    {
        await _userInfoCollection.InsertOneAsync(user);
    }

    public async Task DeleteUser(string userId)
    {
        await _userInfoCollection.DeleteOneAsync(u => u.UserId == userId);
    }

    public async Task<UserInfo> GetUser(string userId)
    {
        var user = await _userInfoCollection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
        return user;
    }
}

