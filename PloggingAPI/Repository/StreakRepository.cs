using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Plogging.Core.Models;
using PloggingAPI.Models;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Repository;

public class StreakRepository : IStreakRepository
{
	private readonly IMongoCollection<UserStreak> _ploggingStreakCollection;

	public StreakRepository(IOptions<PloggingDatabaseSettings> settings)
	{
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(settings.Value.DatabaseName);
		_ploggingStreakCollection = mongoDataBase.GetCollection<UserStreak>(settings.Value.StreakCollectionName);
	}

	// called when user ends plogging session
	public async Task UpdateStreak(string id)
	{
        var user = await _ploggingStreakCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();

		DateTime currentDate = DateTime.UtcNow;
        DateTime currentDateMidnight = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, DateTimeKind.Utc);

        DateTime currentWeekStartDate = currentDateMidnight.AddDays(-(int)currentDateMidnight.DayOfWeek + (int)DayOfWeek.Monday);

		if (user.LastPlogged < currentWeekStartDate)
		{
			user.Streak += 1;
		}

		user.LastPlogged = currentDateMidnight;
		await _ploggingStreakCollection.ReplaceOneAsync(u => u.UserId == id, user);
    }

	// called when user logs in
	public async Task ResetStreak(string id)
	{
        var user = await _ploggingStreakCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();

		DateTime currentDate = DateTime.UtcNow;
		DateTime currentDateMidnight = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, DateTimeKind.Utc);

        DateTime previousWeekStartDate = currentDateMidnight.AddDays(-(int)currentDateMidnight.DayOfWeek + (int)DayOfWeek.Monday - 7);

        if (user.LastPlogged < previousWeekStartDate)
        {
            user.Streak = 0;
            await _ploggingStreakCollection.ReplaceOneAsync(u => u.UserId == id, user);
        }
    }

	public async Task<UserStreak> GetUserStreak(string id)
	{
        var user = await _ploggingStreakCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();

		return user;
    }




    //------------- temporary methods ------------

	public async Task<UserStreak> CreateUser(UserStreak user)
	{
		await _ploggingStreakCollection.InsertOneAsync(user);
		return user;
	}
}