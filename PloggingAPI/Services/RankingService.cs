using Plogging.Core.Models;
using PloggingAPI.Repository.Interfaces;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Services;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _rankingRepository;
    private readonly IPloggingSessionRepository _ploggingSessionRepository;

    public RankingService(IRankingRepository rankingRepository, IPloggingSessionRepository ploggingSessionRepository)
    {
        _rankingRepository = rankingRepository;
        _ploggingSessionRepository = ploggingSessionRepository;
    }

    //TODO Get all rankings over a specific TIME PERIOD
    public async Task<IEnumerable<UserRanking>> GetRankings(int pageNumber, int pageSize)
    {
        var rankings = await _rankingRepository.GetUserRankings(pageNumber, pageSize);

        return rankings;
    }

    //Todo run if it hasnt run in some hours
    //todo Performant enough?
    public async Task UpdateUserRankings()
    {
        var sessions = await _ploggingSessionRepository.GetSessionSummaries();
        var userRanks = await _rankingRepository.GetAllUserRankings();

        var rankings = new List<UserRanking>();
        var rank = 1;
        foreach (var session in sessions)
        {
            var userRank = userRanks.FirstOrDefault(userRank => userRank.Id == session.UserId);
            if (userRank == null)
                continue;

            userRank.Rank = rank++;
            rankings.Add(userRank);
        }

        await _rankingRepository.UpdateUserRankings(rankings);
    }
}
