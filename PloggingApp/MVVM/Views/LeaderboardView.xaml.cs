using Microsoft.Maui.Controls;
using PloggingApp.Pages;
using PloggingApp.MVVM.ViewModels;
using Plogging.Core.Models;

namespace PloggingApp.MVVM.Views;

public partial class LeaderboardView : ContentView
{
    public LeaderboardView()
    {
        InitializeComponent();
    }

    void GoToOtherUserProfile(object sender, TappedEventArgs args)
    {
        UserRanking UserRank = ((LeaderboardViewModel)BindingContext).GetUserRank();
        OthersProfilePageViewModel vm = new OthersProfilePageViewModel(UserRank);
        Navigation.PushAsync(new OthersProfilePage(vm, UserRank));
    }
}
