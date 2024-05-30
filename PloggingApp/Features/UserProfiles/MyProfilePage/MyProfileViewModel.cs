//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using PloggingApp.Extensions;
//using System.Collections.ObjectModel;
//using PloggingApp.Features.Leaderboard;
//using PloggingApp.Shared;
//using PloggingApp.Features.Authentication;
//using PloggingApp.Features.PloggingSession;
//using PloggingApp.Features.Streak;
//using PloggingApp.Features.Statistics;
//using PloggingApp.Features.UserProfiles.Badges;
//using CommunityToolkit.Maui.Core;
//using PlogPal.Domain.Models;

//namespace PloggingApp.Features.UserProfiles;

//public partial class MyProfileViewModel : BaseViewModel, IAsyncInitialization
//{
//    private readonly IPloggingSessionService _ploggingSessionService;
//    private readonly IUserInfoService _userInfoService;
//    private readonly IAuthenticationService _authenticationService;
//    private readonly IRankingService _rankingService;
//    private readonly IStreakService _streakService;
//    private readonly IToastService _toastService;
//    private readonly IPopupService _popupService;

//    //TODO ploggingsessionviewmodel and leaderboardviewmodel should be removed
//    [ObservableProperty]
//    private PloggingSessionViewModel ploggingSessionViewModel;

//    [ObservableProperty]
//    private StreakViewModel streakViewModel;

//    [ObservableProperty]
//    private LeaderboardViewModel leaderboardViewModel;

//    [ObservableProperty]
//    private BadgesViewModel badgesViewModel;
//    private ObservableCollection<PlogSession> PloggingSessions { get; set; } = [];
//    private IEnumerable<PlogSession> _allUserSessions = [];

//    [ObservableProperty]
//    private PloggingStatistics ploggingStatistics;
//    [ObservableProperty]
//    private string displayName;
//    [ObservableProperty]
//    private double totalDistance;
//    [ObservableProperty]
//    private double totalCO2Saved;
//    [ObservableProperty]
//    private double totalWeight;
//    [ObservableProperty]
//    private int userRankInt;
//    [ObservableProperty]
//    private bool isRefreshing;
//    [ObservableProperty]
//    private IEnumerable<PlogSession> latestSessions;

//    public MyProfileViewModel(IAuthenticationService authenticationService)
//    {
//        _authenticationService = authenticationService;

//        Initialization = InitializeAsync();
//    }

//    public Task Initialization { get; private set; }

//    private async Task InitializeAsync()
//    {
//        await GetSessions();
//    }
//    public async Task GetSessions()
//    {
//        IsBusy = true;

//        DisplayName = _authenticationService.CurrentUser.Info.DisplayName;

//        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.UserId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);

//        if (!_allUserSessions.Any())
//        {
//            IsBusy = false;
//            return;
//        }
//        else
//        {
//            PloggingSessions.ClearAndAddRange(_allUserSessions);
//            PloggingStatistics = new PloggingStatistics(_allUserSessions.Where(s => s.StartDate.Month == DateTime.Now.Month));
//            LatestSessions = PloggingSessions.Take(3);

//            //TODO get from RankingService instead, needs to calculate rank
//            UserRankInt = _rankingService.UserRank.Rank;

//            IsBusy = false;
//        }
//    }

//    [RelayCommand]
//    private async Task Logout()
//    {
//        bool response = await Application.Current.MainPage.DisplayAlert("Signing out", "Are you sure you want to logout?", "Yes", "No");

//        if (response)
//        {
//            _authenticationService.SignOut();
//            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
//        }
//    }

//    [RelayCommand]
//    private async Task Refresh()
//    {
//        IsBusy = true;
//        await GetSessions();
//        IsRefreshing = false;
//        IsBusy = false;
//    }

//    [RelayCommand]
//    private async Task GoToHistoryPage()
//    {
//        await Shell.Current.GoToAsync($"{nameof(HistoryPage)}");
//    }
//}

