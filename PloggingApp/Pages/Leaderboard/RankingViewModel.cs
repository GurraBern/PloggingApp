using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages.Leaderboard;

public class RankingViewmodel
{
    public LeaderboardViewModel LeaderboardViewModel { get; set; }
    public RankingViewmodel(LeaderboardViewModel leaderboardViewModel)
    {
        LeaderboardViewModel = leaderboardViewModel;
    }
}
