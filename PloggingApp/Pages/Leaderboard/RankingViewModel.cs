using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public class RankingViewmodel
{
    public LeaderboardViewModel LeaderboardViewModel { get; set; }
    public RankingViewmodel(LeaderboardViewModel leaderboardViewModel)
    {
        LeaderboardViewModel = leaderboardViewModel;
    }

}
