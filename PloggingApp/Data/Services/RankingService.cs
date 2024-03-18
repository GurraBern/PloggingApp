using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Factories;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Services;

namespace PloggingApp.Data.Services;

public class RankingService : IRankingService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient;
    private readonly IToastService _toastService;

    public RankingService(IPloggingApiClient<PloggingSession> ploggingApiClient, IToastService toastService)
    {
        _ploggingApiClient = ploggingApiClient;
        _toastService = toastService;
    }

    public async Task<IEnumerable<UserRanking>> GetUserRankings(DateTime startDate, DateTime endDate, SortProperty sortProperty)
    {
        try
        {
            var sessionsRequest = SessionRequestFactory.CreateRequest(startDate, endDate, SortDirection.Descending, sortProperty);

            var ploggingSummaries = await _ploggingApiClient.GetAllAsync(sessionsRequest);

            var rankings = new List<UserRanking>();
            var rank = 1;
            foreach (var summary in ploggingSummaries)
            {
                var userRank = new UserRanking()
                {
                    Id = summary.UserId,
                    DisplayName = summary.DisplayName,
                    PloggingData = summary.PloggingData,
                    Rank = rank++
                };

                rankings.Add(userRank);
            }

            return rankings;
        }
        catch (Exception)
        {
            await _toastService.MakeToast("Could not fetch user rankings"); 
            return Enumerable.Empty<UserRanking>();
        }
    }
}
