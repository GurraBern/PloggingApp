namespace PlogPal.Application.Common.Interfaces;

public interface IStreakManager
{
    Task<int> GetStreak(string userId);
}