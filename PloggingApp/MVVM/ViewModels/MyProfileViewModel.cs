using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Authentication;
using System.Diagnostics;
using PloggingApp.Pages;
using PloggingApp.Data.Services;

namespace PloggingApp.MVVM.ViewModels;

public partial class MyProfileViewModel : ObservableObject, IAsyncInitialization
{
    private readonly IAuthenticationService _authenticationService;
    public StreakViewModel StreakViewModel { get; set; }
    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StatisticsViewModel StatisticsViewModel { get; }
    public Task Initialization { get; }

    public MyProfileViewModel(IAuthenticationService authenticationService, StreakViewModel streakViewModel, PloggingSessionViewModel ploggingSessionViewModel, StatisticsViewModel statisticsViewModel)
    {
        _authenticationService = authenticationService;
        PloggingSessionViewModel = ploggingSessionViewModel;
        StreakViewModel = streakViewModel;
        StatisticsViewModel = statisticsViewModel;

        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        Debug.WriteLine("MyProfileViewModel initialized.");
    }

    [RelayCommand]
    private async Task Logout()
    {
        _authenticationService.SignOut();
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }


}