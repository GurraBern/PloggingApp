using System;
using Plogging.Core.Models;

namespace PloggingApp.Data.Services;

public interface IStreakService
{
    Task<UserStreak> GetUserStreak(string id);
}


