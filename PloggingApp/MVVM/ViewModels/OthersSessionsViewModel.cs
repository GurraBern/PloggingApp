using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using PloggingApp.MVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Data.Services;
using CommunityToolkit.Maui.Core;
using PloggingApp.Shared;

namespace PloggingApp.MVVM.ViewModels;

public partial class OthersSessionsViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly string userId;
    private readonly IPloggingSessionService _sessionService;
    private readonly IUserInfoService _userInfo;
    private readonly IStreakService _streakService;

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];
    public ObservableCollection<Badge> Badges { get; set; } = [];

    [ObservableProperty]
    private PloggingStatistics ploggingStatistics;

    [ObservableProperty]
    private string displayName;

    [ObservableProperty]
    private string imageURI;

    [ObservableProperty]
    private string streakString;

    private IEnumerable<PloggingSession> _allSessions = [];
    public BadgesViewModel BadgesViewModel { get; set; }


    public Task Initialization { get; private set; }

    public OthersSessionsViewModel(string userId, IPloggingSessionService sessionService, IUserInfoService userInfo, IStreakService streakService, IPopupService popupService)
    {
        this.userId = userId;
        _sessionService = sessionService;
        _userInfo = userInfo;
        _streakService = streakService;

        BadgesViewModel = new BadgesViewModel(sessionService, userInfo, streakService, popupService);

        Initialization = GetSessions();
    }

    public async Task GetSessions()
    {
        IsBusy = true;

        if(userId != null)
        {
            var user = await _userInfo.GetUser(userId);
            DisplayName = user.DisplayName;
            _allSessions = await _sessionService.GetUserSessions(userId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);

            PloggingStatistics = new PloggingStatistics(_allSessions);
            PloggingStatistics.TotalSteps = Math.Round(PloggingStatistics.TotalSteps,1);
            PloggingStatistics.TotalDistance = Math.Round(PloggingStatistics.TotalDistance,1);
            PloggingStatistics.TotalWeight = Math.Round(PloggingStatistics.TotalWeight,1);
            PloggingStatistics.TotalCO2Saved = Math.Round(PloggingStatistics.TotalCO2Saved,1);

            foreach (PloggingSession ps in _allSessions)
            {
                ps.PloggingData.Distance = Math.Round(ps.PloggingData.Distance,1);
                ps.PloggingData.Weight = Math.Round(ps.PloggingData.Weight,1);
                ps.StartDate = ps.StartDate.AddHours(2);
            }

            PloggingSessions.ClearAndAddRange(_allSessions);
            int streak = (await _streakService.GetUserStreak(userId)).Streak;
            StreakString = streak.ToString();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("ERROR","Cant Show Profile", "OK");
        }

        IsBusy = false;
    }
}