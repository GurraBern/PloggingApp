using Plogging.Core.Models;

namespace PloggingAPI.Services;

public interface IRankingService
{
    Task<IEnumerable<UserRanking>> GetRankings();
}
