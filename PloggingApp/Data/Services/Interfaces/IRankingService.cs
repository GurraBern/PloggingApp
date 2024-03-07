using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Data.Services;

public interface IRankingService
{
    Task<IEnumerable<UserRanking>> GetUserRankings(DateTime startDate, DateTime endDate, SortProperty sortProperty = SortProperty.Weight);
}