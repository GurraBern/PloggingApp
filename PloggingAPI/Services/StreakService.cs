using Plogging.Core.Models;
using PloggingAPI.Repository.Interfaces;
using PloggingAPI.Services.Interfaces;


namespace PloggingAPI.Services;


public class StreakService : IStreakService
{
    private readonly IStreakRepository _streakRepository;

    public StreakService(IStreakRepository streakRepository)
    {
        _streakRepository = streakRepository;
    }

    public async Task UpdateStreak(string userId)
    {
        await _streakRepository.UpdateStreak(userId);
    }

    public async Task ResetStreak(string userId)
    {
        await _streakRepository.ResetStreak(userId);
    }

    public async Task<UserStreak> GetUserStreak(string userId)
    {
        UserStreak user = await _streakRepository.GetUserStreak(userId);
        return user;
    }

    public async Task<UserStreak> CreateUser(UserStreak user)
    {
        await _streakRepository.CreateUser(user);
        return user;
    }
}

