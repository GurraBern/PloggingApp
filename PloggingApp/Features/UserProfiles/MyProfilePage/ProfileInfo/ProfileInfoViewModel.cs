using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Features.Authentication;
using PloggingApp.Features.Leaderboard;
using PloggingApp.Features.Streak;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;

namespace PloggingApp.Features.UserProfiles;

public partial class ProfileInfoViewModel : ObservableObject, IAsyncInitialization
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IRankingService _rankingService;

    [ObservableProperty]
    private StreakViewModel streakViewModel;

    public string DisplayName { get; private set; }

    [ObservableProperty]
    private UserRanking userRank;

    public Task Initialization { get; private set; }

    public ProfileInfoViewModel(IAuthenticationService authenticationService, IRankingService rankingService, StreakViewModel streakViewModel)
    {
        _authenticationService = authenticationService;
        _rankingService = rankingService;
        StreakViewModel = streakViewModel;

        DisplayName = _authenticationService.CurrentUser.Info.DisplayName;

        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _rankingService.InitializeAsync();
        UserRank = _rankingService.UserRank;
    }

    [RelayCommand]
    private async Task Logout()
    {
        bool response = await Application.Current.MainPage.DisplayAlert("Signing out", "Are you sure you want to logout?", "Yes", "No");

        if (response)
        {
            _authenticationService.SignOut();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
