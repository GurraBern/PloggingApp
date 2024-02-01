using PloggingApp.Data.Context.Interfaces;
using PloggingApp.Shared.Models;

namespace PloggingApp.Features.Leaderboard;

public class LeaderboardViewModel
{
    public IEnumerable<Ranking> Rankings { get; }

    public LeaderboardViewModel(IRankingContext rankingContext)
    {
        Rankings = rankingContext.Rankings;
    }
}
