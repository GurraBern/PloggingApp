using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface IRankingRepository
{
    Task<IEnumerable<UserRanking>> GetUserRankings(int pageNumber, int pageSize);
    Task<IEnumerable<UserRanking>> GetAllUserRankings();
    Task UpdateUserRankings(IEnumerable<UserRanking> userRankings);
}