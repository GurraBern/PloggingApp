using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface IStreakRepository
{
    Task<UserStreak> CreateUser(UserStreak user);
    Task UpdateStreak(string id);
    Task ResetStreak(string id);
    Task<UserStreak> GetUserStreak(string id);
}


