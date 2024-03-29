using System;
using Plogging.Core.Models;

namespace PloggingApp.Data.Services;

public interface IStreakService
{
    Task<UserStreak> GetUserStreak(string userId);

    Task UpdateStreak(string userId);

    Task ResetStreak(string userId);

    Task CreateUser(string userId);
}


