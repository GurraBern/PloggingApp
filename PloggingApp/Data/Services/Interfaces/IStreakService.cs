using System;
using Plogging.Core.Models;

namespace PloggingApp.Data.Services;

public interface IStreakService
{
    Task<UserStreak> GetUserStreak(string id);

    Task UpdateStreak(string id);

    Task ResetStreak(string id);

    Task CreateUser(string userId);
}


