using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public class RankingViewmodel
{
    public LeaderboardViewModel LeaderboardViewModel { get; set; }
    public MyProfileViewModel MyProfileViewModel { get; set; }
    public RankingViewmodel(LeaderboardViewModel leaderboardViewModel, MyProfileViewModel myProfileViewModel)
    {
        LeaderboardViewModel = leaderboardViewModel;
        MyProfileViewModel = myProfileViewModel;
    }
}
