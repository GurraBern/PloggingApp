using Plogging.Core.Models;

namespace PloggingAPI.Services.Interfaces;

public interface IRankingService
{
    Task<IEnumerable<UserRanking>> GetRankings(int pageNumber, int pageSize);
}
