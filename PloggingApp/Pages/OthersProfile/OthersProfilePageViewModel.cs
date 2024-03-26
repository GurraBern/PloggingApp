using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;
using RestSharp;

namespace PloggingApp.Pages;

[QueryProperty(nameof(userPassed),"userPassed" )]
public partial class OthersProfilePageViewModel : BaseViewModel, IQueryAttributable
{
    public OthersSessionsViewModel OthersSessionsViewModel { get; set; }

    [ObservableProperty]
    public string userPassed;
    IPloggingSessionService _sessionService;
    IUserInfoService _userService;
    IStreakService _streakService;
    IPopupService _popupService;
    public OthersProfilePageViewModel(IPloggingSessionService sessionService, IUserInfoService userService, IStreakService streakService, IPopupService popupService)
    {
        _sessionService = sessionService;
        _userService = userService;
        _streakService = streakService;
        _popupService = popupService;

    }
    [RelayCommand]
    public async Task GoBack()
    {
        await Shell.Current.GoToAsync($"//{nameof(RankingPage)}");
    }
    private void Init(IPloggingSessionService sessionService, IUserInfoService userService, IStreakService streakService, IPopupService popupService)
    {
        OthersSessionsViewModel = new OthersSessionsViewModel(sessionService, userService, streakService, popupService, userPassed);
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        this.userPassed = query[nameof(userPassed)] as string;
        Init(_sessionService, _userService, _streakService, _popupService);
    }
}
