using Plogging.Core.Models;

namespace PloggingApp.Data.Services;

public interface IRankingService
{
    Task<IEnumerable<UserRanking>> GetUserRankings();
}