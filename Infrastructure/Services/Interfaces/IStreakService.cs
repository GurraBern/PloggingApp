using Plogging.Core.Models;

namespace Infrastructure.Services.Interfaces;

public interface IStreakService
{
    Task<UserStreak> GetUserStreak(string userId);

    Task UpdateStreak(string userId);

    Task ResetStreak();

    Task CreateUser(string userId);
}

