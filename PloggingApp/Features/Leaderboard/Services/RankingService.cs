using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Factories;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;

namespace PloggingApp.Features.Leaderboard;

public class RankingService(IPloggingApiClient<PloggingSession> ploggingApiClient, IAuthenticationService authenticationService) : IRankingService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient = ploggingApiClient;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public UserRanking UserRank { get; set; } = UserRanking.CreateDefault();
    public IEnumerable<UserRanking> UserRankings { get; private set; } = [];

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

            UserRank = rankings.FirstOrDefault(user => user.Id.Equals(_authenticationService.UserId, StringComparison.InvariantCultureIgnoreCase));

            UserRankings = rankings;

            return rankings;
        }
        catch (Exception)
        {
            return [];
        }
    }
}
