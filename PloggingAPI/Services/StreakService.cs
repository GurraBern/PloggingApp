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

    public async Task UpdateStreak(string id)
    {
        await _streakRepository.UpdateStreak(id);
    }

    public async Task ResetStreak(string id)
    {
        await _streakRepository.ResetStreak(id);
    }

    public async Task<UserStreak> GetUserStreak(string id)
    {
        UserStreak user = await _streakRepository.GetUserStreak(id);
        return user;
    }

    public async Task<UserStreak> CreateUser(UserStreak user)
    {
        await _streakRepository.CreateUser(user);
        return user;
    }
}

