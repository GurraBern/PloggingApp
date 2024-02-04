using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using RestSharp;

namespace PloggingApp.Data.Services;

public class RankingService : IRankingService
{
    private readonly IPloggingApiClient<UserRanking> _ploggingApiClient;

    public RankingService(IPloggingApiClient<UserRanking> ploggingApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
    }

    public async Task<IEnumerable<UserRanking>> GetUserRankings()
    {
        var request = new RestRequest("api/UserRanking");

        return await _ploggingApiClient.GetAllAsync(request);
    }
}
