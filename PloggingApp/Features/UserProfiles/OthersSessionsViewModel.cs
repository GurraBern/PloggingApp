using System.Collections.ObjectModel;
using PloggingApp.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Core;
using PloggingApp.Shared;
using PloggingApp.Features.Statistics;
using PloggingApp.Features.UserProfiles.Badges;
using PlogPal.Domain.Models;

namespace PloggingApp.Features.UserProfiles;

public partial class OthersSessionsViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly string _userId;
    private readonly IPloggingSessionService _sessionService;
    private readonly IUserInfoService _userInfo;
    private readonly IStreakService _streakService;

    public ObservableCollection<PlogSession> PloggingSessions { get; set; } = [];
    public ObservableCollection<Badge> Badges { get; set; } = [];

    [ObservableProperty]
    private PloggingStatistics ploggingStatistics;

    [ObservableProperty]
    private string displayName;

    [ObservableProperty]
    private string imageURI;

    [ObservableProperty]
    private string streakString;

    private IEnumerable<PlogSession> _allSessions = [];

    [ObservableProperty]
    public BadgesViewModel badgesViewModel;


    public Task Initialization { get; private set; }

    public OthersSessionsViewModel(string userId, IPloggingSessionService sessionService, IUserInfoService userInfo, IStreakService streakService)
    {
        _userId = userId;
        _sessionService = sessionService;
        _userInfo = userInfo;
        _streakService = streakService;

        //BadgesViewModel = new BadgesViewModel(sessionService, userInfo, streakService, popupService);

        Initialization = GetSessions();
    }

    public async Task GetSessions()
    {
        IsBusy = true;

        if(_userId != null)
        {
            var user = await _userInfo.GetUser(_userId);
            DisplayName = user.DisplayName;
            _allSessions = await _sessionService.GetUserSessions(_userId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);

            PloggingStatistics = new PloggingStatistics(_allSessions);
            PloggingStatistics.TotalSteps = Math.Round(PloggingStatistics.TotalSteps,1);
            PloggingStatistics.TotalDistance = Math.Round(PloggingStatistics.TotalDistance,1);
            PloggingStatistics.TotalWeight = Math.Round(PloggingStatistics.TotalWeight,1);
            PloggingStatistics.TotalCO2Saved = Math.Round(PloggingStatistics.TotalCO2Saved,1);

            foreach (PlogSession ps in _allSessions)
            {
                ps.PloggingData.Distance = Math.Round(ps.PloggingData.Distance,1);
                ps.PloggingData.Weight = Math.Round(ps.PloggingData.Weight,1);
                ps.StartDate = ps.StartDate.AddHours(2);
            }

            PloggingSessions.ClearAndAddRange(_allSessions);
            int streak = (await _streakService.GetUserStreak(_userId)).Streak;
            StreakString = streak.ToString();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("ERROR","Cant Show Profile", "OK");
        }

        IsBusy = false;
    }
}