using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using CommunityToolkit.Mvvm.Input;

using Plogging.Core.Enums;
using PloggingApp.MVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Data.Services;
using Firebase.Auth.Requests;
using Firebase.Auth;

namespace PloggingApp.MVVM.ViewModels;

public partial class OthersSessionsViewModel : BaseViewModel, IAsyncInitialization
{

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];

    public ObservableCollection<Badge> Badges { get; set; } = [];
    public List<Badge> _Badges { get; set; } = [];

    [ObservableProperty]
    public double totalSteps;
    [ObservableProperty]
    public double totalDistance;
    [ObservableProperty]
    public double totalCO2Saved;
    [ObservableProperty]
    public double totalWeight;
    [ObservableProperty]
    public string displayName;


    private IEnumerable<PloggingSession> _allSessions = new ObservableCollection<PloggingSession>();

    private readonly IPloggingSessionService _sessionService;
    private readonly IUserInfoService _userInfo;
    private IRelayCommand? RecentSessionCommand { get; set; }
    public Task Initialization { get; private set; }

    public OthersSessionsViewModel(IPloggingSessionService SessionService, IUserInfoService UserInfo)
    {
        _sessionService = SessionService;
        _userInfo = UserInfo;
        Initialization =GetSessionsAndBadges(); //Dela upp i två funktioner?
    }

    [RelayCommand]
    public async Task UpdatePage()
    {

        await GetSessionsAndBadges();

    }

    public async Task GetSessionsAndBadges()
    {
        IsBusy = true;
        var userId = _sessionService.UserId;
        var user = await _userInfo.GetUser(userId);
        DisplayName = user.DisplayName;
        _allSessions = await _sessionService.GetUserSessions(userId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        var stats = new PloggingStatistics(_allSessions);
        TotalSteps = Math.Round(stats.TotalSteps);
        TotalDistance = Math.Round(stats.TotalDistance);
        TotalCO2Saved = Math.Round(stats.TotalCO2Saved);
        TotalWeight = Math.Round(stats.TotalWeight);
        PloggingSessions.ClearAndAddRange(_allSessions);
        await GetBadges(userId, _allSessions, stats);
        IsBusy = false;
    }

    public async Task GetBadges(string UserId, IEnumerable<PloggingSession> _allSessions, PloggingStatistics stats)
    {

        _Badges.Add(new TrashCollectedBadge(stats));
        _Badges.Add(new DistanceBadge(stats));
        _Badges.Add(new TimeSpentBadge(stats));
        Badges.ClearAndAddRange(_Badges);
        _Badges.Clear();
    }





}