using PlogPal.Domain.Models;

namespace PloggingAPI.Features.Streak;

public interface IStreakService
{
    Task CreateUser(UserStreak user);
    Task UpdateStreak(string id);
    Task ResetStreak(string id);
    Task<UserStreak> GetUserStreak(string id);
}

