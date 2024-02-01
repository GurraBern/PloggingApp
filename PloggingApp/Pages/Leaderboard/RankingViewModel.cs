using PloggingApp.Features.Leaderboard;

namespace PloggingApp.Pages.Leaderboard;

public class RankingViewmodel
{
    public LeaderboardViewModel LeaderboardViewModel { get; set; }
    public RankingViewmodel(LeaderboardViewModel leaderboardViewModel)
    {
        LeaderboardViewModel = leaderboardViewModel;
    }
}
