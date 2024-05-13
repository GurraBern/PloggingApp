using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Factories;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Services;
using PloggingApp.Services.Authentication;

namespace PloggingApp.Features.Leaderboard;

public class RankingService : IRankingService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient;
    private readonly IToastService _toastService;
    private readonly IAuthenticationService _authenticationService;

    public RankingService(IPloggingApiClient<PloggingSession> ploggingApiClient, IToastService toastService, IAuthenticationService authenticationService)
    {
        _ploggingApiClient = ploggingApiClient;
        _toastService = toastService;
        _authenticationService = authenticationService;
    }

    public async Task<IEnumerable<UserRanking>> GetUserRankings(DateTime startDate, DateTime endDate, SortProperty sortProperty)
    {
        try
        {
            var sessionsRequest = SessionRequestFactory.CreateRequest(startDate, endDate, SortDirection.Descending, sortProperty);

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            var ploggingSummaries = await _ploggingApiClient.GetAllAsync(sessionsRequest, bearerToken);

            var rankings = new List<UserRanking>();
            var rank = 1;
            foreach (var summary in ploggingSummaries)
            {
                summary.PloggingData.Weight = Math.Round(summary.PloggingData.Weight, 1);
                summary.PloggingData.Distance = Math.Round(summary.PloggingData.Distance);
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
            return [];
        }
    }
}
