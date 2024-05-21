using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Shared;

namespace PloggingApp.Features.UserProfiles;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class OthersProfilePageViewModel(IPloggingSessionService sessionService, IUserInfoService userService, IStreakService streakService, IPopupService popupService) : BaseViewModel
{
    [ObservableProperty]
    public OthersSessionsViewModel? othersSessionsViewModel;
    
    [ObservableProperty]
    private string userId = "";
    private readonly IPloggingSessionService sessionService = sessionService;
    private readonly IUserInfoService userService = userService;
    private readonly IStreakService streakService = streakService;
    private readonly IPopupService popupService = popupService;

    public void InitializeComponents()
    {
        OthersSessionsViewModel = new OthersSessionsViewModel(UserId, sessionService, userService, streakService, popupService);
    }

    [RelayCommand]
    public async Task GoBack()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"..");
        IsBusy = false;
    }
}
