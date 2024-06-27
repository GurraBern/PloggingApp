using PlogPal.Domain.Models;

namespace PlogPal.Application.Common.Interfaces;

public interface IStreakService
{
    Task<UserStreak> GetUserStreak(string userId);

    Task UpdateStreak(string userId);

    Task ResetStreak();

    Task CreateUser(string userId);
}

