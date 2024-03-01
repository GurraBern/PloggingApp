using Plogging.Core.Models;

public interface IStreakService
{
    Task<UserStreak> CreateUser(UserStreak user);
    Task UpdateStreak(string id);
    Task ResetStreak(string id);
    Task<UserStreak> GetUserStreak(string id);
}

