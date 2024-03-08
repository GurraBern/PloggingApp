using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Services.Authentication;


namespace PloggingApp.MVVM.ViewModels;

public partial class StreakViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly IStreakService _streakService;
    private readonly IAuthenticationService _authenticationService;

    private IRelayCommand? RecentStreakCommand { get; set; }
    public Task Initialization { get; private set; }

    [ObservableProperty]
    private UserStreak userStreakCount;

    public StreakViewModel(IStreakService streakService, IAuthenticationService authenticationService)
	{
        _streakService = streakService;
        _authenticationService = authenticationService;
        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await GetUserStreak();
    }

    [RelayCommand]
    private async Task GetUserStreak()
    {
        var currentUserId = _authenticationService.CurrentUser.Uid; 

        RecentStreakCommand = GetUserStreakCommand;
        IsBusy = true;
        UserStreakCount = await _streakService.GetUserStreak(currentUserId);
        IsBusy = false;
        
        
    }
}

