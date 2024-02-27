using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;

namespace PloggingApp.MVVM.ViewModels;

public partial class StreakViewModel : BaseViewModel
{
    private readonly IStreakService _streakService;

    private IRelayCommand? RecentStreakCommand { get; set; }

    [ObservableProperty]
    private UserStreak userStreakCount;

    public StreakViewModel(IStreakService streakService)
	{
        _streakService = streakService;
    }


    [RelayCommand]
    private async Task GetUserStreak()
    {
        //TODO replace with actual id when user authentication is implemented
        var currentUserId = "333ajsldkfjasödjfk34"; 

        RecentStreakCommand = GetUserStreakCommand;
        IsBusy = true;
        UserStreakCount = await _streakService.GetUserStreak(currentUserId);
        IsBusy = false;
        
        
    }
}

