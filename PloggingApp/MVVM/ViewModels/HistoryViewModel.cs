using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Authentication;
using PloggingApp.Extensions;
using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models;
using PloggingApp.Pages;
using PloggingApp.Services;
using System.Windows.Input;
using PloggingApp.Services.SessionStatistics;

namespace PloggingApp.MVVM.ViewModels;

public partial class HistoryViewModel : BaseViewModel, IAsyncInitialization
{
    public Task Initialization { get; set; }

    private readonly IToastService _toastService;
    private readonly IPloggingSessionService _ploggingSessionService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ISessionStatisticsService _sessionStatisticsService;


    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StatisticsViewModel StatisticsViewModel { get; }

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];

    public IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();

    [ObservableProperty]
    bool isRefreshing;
    public HistoryViewModel(IPloggingSessionService ploggingSessionService,
        PloggingSessionViewModel ploggingSessionViewModel, IToastService toastService, 
        IAuthenticationService authenticationService, StatisticsViewModel statisticsViewModel,
        ISessionStatisticsService sessionStatisticsService)
	{
        _ploggingSessionService = ploggingSessionService;
        PloggingSessionViewModel = ploggingSessionViewModel;
        _toastService = toastService;
        _authenticationService = authenticationService;
        StatisticsViewModel = statisticsViewModel;
        _sessionStatisticsService = sessionStatisticsService;


        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await GetSessions();
    }
    public async Task GetSessions() {

        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.CurrentUser.Uid, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        if (!_allUserSessions.Any())
        {
            await _toastService.MakeToast("No sessions found :(", CommunityToolkit.Maui.Core.ToastDuration.Short);
            IsBusy = false;
            return;
        }
        PloggingSessions.ClearAndAddRange(_allUserSessions);

        IsBusy = false;
    }

    [RelayCommand]
    private async Task GoToSessionStats(PloggingSession session)
    {
        await _sessionStatisticsService.GoToSessionStats(session);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        // IsRefreshing = true
        IsBusy = true;
        GetSessions();
        IsRefreshing = false;
        IsBusy = false;
    }
}