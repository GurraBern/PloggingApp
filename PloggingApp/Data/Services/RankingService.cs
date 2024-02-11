using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using RestSharp;

namespace PloggingApp.Data.Services;

public class RankingService : IRankingService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient;

    public RankingService(IPloggingApiClient<PloggingSession> ploggingApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
    }

    public async Task<IEnumerable<UserRanking>> GetUserRankings()
    {
        try
        {
            var request = new RestRequest("api/PloggingSession/Summary");
            request.AddParameter("startDate", DateTime.Now.AddDays(-5));
            request.AddParameter("endDate", DateTime.Now);
            request.AddParameter(nameof(SortDirection), SortDirection.Descending);
            request.AddParameter(nameof(SortProperty), SortProperty.ScrapCount);

            //TODO add time interval sorting etc
            var ploggingSummaries = await _ploggingApiClient.GetAllAsync(request);

            var rankings = new List<UserRanking>();
            var rank = 1;
            foreach (var summary in ploggingSummaries)
            {
                var userRank = new UserRanking()
                {
                    DisplayName = "test",
                    Id = summary.UserId,
                    Rank = rank++,
                    PloggingData = summary.PloggingData
                };

                rankings.Add(userRank);
            }

            return rankings;
        }
        catch (Exception ex)
        {
            //TODO display toast
            return Enumerable.Empty<UserRanking>();
        }
    }
}
