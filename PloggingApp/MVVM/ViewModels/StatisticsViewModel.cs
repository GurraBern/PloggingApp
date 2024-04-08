using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using Plogging.Core.Models;
using Plogging.Core.Enums;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using System.Collections.ObjectModel;
using PloggingApp.Services.Statistics;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Pages;

namespace PloggingApp.MVVM.ViewModels;

public partial class StatisticsViewModel : BaseViewModel, IAsyncInitialization
{

    public Task Initialization { get; private set; }

    private readonly IPloggingSessionService _ploggingSessionService;
    private IChartService chartService;
    private readonly IAuthenticationService _authenticationService;
    public ObservableCollection<PloggingSession> UserSessions { get; set; } = [];
    private IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();
    private Dictionary<TimeResolution, string> colorDict = new Dictionary<TimeResolution, string>
    {
        {TimeResolution.ThisYear,"#5c5aa8" },
        {TimeResolution.ThisMonth, "#9558a8"}
    };
    private bool _isInitialized = false;
    private int _selectedYear;
    private int _selectedMonth;
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value;
            
            if (!_isInitialized)
                return;
            Update();
        }
    }
    public int SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            _selectedMonth = value;
           if (!_isInitialized)
                return;
            Update();
        }
    }

    public StatisticsViewModel(IPloggingSessionService ploggingSessionService, IAuthenticationService authenticationService)
    {
        _ploggingSessionService = ploggingSessionService;
        _authenticationService = authenticationService;
        Initialization = InitializeAsync();
        Years = new ObservableCollection<int>(Enumerable.Range(DateTime.UtcNow.Year - 2, 3));
        Months = new ObservableCollection<int>(Enumerable.Range(1, 12));
        TimeRes = TimeResolution.ThisYear;
        StatsBoxColor = colorDict[TimeRes];
        SelectedYear = DateTime.UtcNow.Year;
        SelectedMonth = 4;
        

    }
    private async Task InitializeAsync()
    {
        await GetUserSessions();
    }
    private async Task GetUserSessions()
    {
        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.CurrentUser.Uid, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        UserSessions.ClearAndAddRange(_allUserSessions);
        chartService = new ChartService();
        PloggingStats = new PloggingStatistics(UserSessions);
        DistanceChart = new ChartContext
        {
            Chart = chartService.generateDistanceChart(TimeRes,UserSessions, SelectedYear, SelectedMonth),
            Name = "Distance",
            Unit = "m"
        };
        LitterChart = new ChartContext
        {
            Chart = chartService.generateLitterChart(TimeRes, UserSessions),
            Name = "Litter",
            Unit = "pcs"
        };
        _isInitialized = true;
        IsBusy = false;
    }
    [RelayCommand]
    private void ShowMonth()
    {
        TimeRes = TimeResolution.ThisMonth;
        Update();
    }

    [RelayCommand]
    private void ShowYear()
    {
        TimeRes = TimeResolution.ThisYear;
        Update();
    }
    private void Update()
    {
        IsBusy = true;
        if (TimeRes is TimeResolution.ThisYear)
        {
            UserSessions.ClearAndAddRange(_allUserSessions.Where(s => s.StartDate.Year == SelectedYear));
        }
        else
        {
            UserSessions.ClearAndAddRange(_allUserSessions.Where(s => s.StartDate.Year == SelectedYear && s.StartDate.Month == SelectedMonth));
        }
        LitterChart.Chart = chartService.generateLitterChart(TimeRes, UserSessions);
        DistanceChart.Chart = chartService.generateDistanceChart(TimeRes, UserSessions, SelectedYear, SelectedMonth);
        PloggingStats = new PloggingStatistics(UserSessions);
        PloggingStats.changeTimeResolution(TimeRes);
        StatsBoxColor = colorDict[TimeRes];
        IsBusy = false;
    }
    [RelayCommand]
    private async Task GoToSessionStats(PloggingSession session)
    {
        if (session is null)
            return;
        await Shell.Current.GoToAsync($"{nameof(SessionStatisticsPage)}", true, 
            new Dictionary<string, object>
            {
                {nameof(PloggingSession), session}
            });
    }

    [RelayCommand]
    private async Task Refresh()
    {
        // IsRefreshing = true
        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.CurrentUser.Uid, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        Update();
        IsRefreshing = false;
        IsBusy = false;
    }

    [ObservableProperty]
    ObservableCollection<int> months;

    [ObservableProperty]
    ObservableCollection<int> years;

    [ObservableProperty]
    int filterYear;

    [ObservableProperty]
    int filterMonth;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    ChartContext distanceChart;

    [ObservableProperty]
    ChartContext litterChart;

    [ObservableProperty]
    TimeResolution timeRes;

    [ObservableProperty]
    PloggingStatistics ploggingStats;

    [ObservableProperty]
    string statsBoxColor;
}
