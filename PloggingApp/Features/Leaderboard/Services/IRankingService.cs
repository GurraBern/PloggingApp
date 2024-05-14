using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Features.Leaderboard;

public interface IRankingService
{
    Task<IEnumerable<UserRanking>> GetUserRankings(DateTime startDate, DateTime endDate, SortProperty sortProperty = SortProperty.Weight);
    public IEnumerable<UserRanking> UserRankings { get; } 
    UserRanking UserRank { get; }
}