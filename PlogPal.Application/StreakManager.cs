using PlogPal.Application.Common.Interfaces;

namespace PlogPal.Application;

public class StreakManager : IStreakManager
{
    private readonly IStreakService _streakService;

    public StreakManager(IStreakService streakService)
    {
        _streakService = streakService;
    }
    
    public async Task<int> GetStreak(string userId)
    {
        var userStreak = await _streakService.GetUserStreak(userId);
        return userStreak.Streak;
    }
}