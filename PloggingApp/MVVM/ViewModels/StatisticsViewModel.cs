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
using System;
using PloggingApp.Services;

namespace PloggingApp.MVVM.ViewModels;

public partial class StatisticsViewModel : BaseViewModel, IAsyncInitialization
{

    public Task Initialization { get; private set; }

    private readonly IPloggingSessionService _ploggingSessionService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IChartService _chartService;
    private readonly IToastService _toastService;
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
            _selectedMonth = value + 1;
           if (!_isInitialized)
                return;
            Update();
        }
    }

    public StatisticsViewModel(IPloggingSessionService ploggingSessionService, IAuthenticationService authenticationService, IChartService chartService, IToastService toastService)
    {
        _ploggingSessionService = ploggingSessionService;
        _authenticationService = authenticationService;
        _chartService = chartService;
        _toastService = toastService;
        
        Years = new ObservableCollection<int>(Enumerable.Range(DateTime.UtcNow.Year - 2, 3));
        Months = new ObservableCollection<string>(Enum.GetNames(typeof(Month)).ToList());
        TimeRes = TimeResolution.ThisYear;
        StatsBoxColor = colorDict[TimeRes];
        SelectedYear = DateTime.UtcNow.Year;
        SelectedMonth = DateTime.UtcNow.Month - 2;
        Initialization = InitializeAsync();
    }
    private async Task InitializeAsync()
    {
        await GetUserSessions();
    }
    private async Task GetUserSessions()
    {
        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.CurrentUser.Uid, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        if (!_allUserSessions.Any())
            await _toastService.MakeToast("No sessions found :(", CommunityToolkit.Maui.Core.ToastDuration.Short);
        UserSessions.ClearAndAddRange(_allUserSessions);
        PloggingStats = new PloggingStatistics(UserSessions);
        DistanceChart = new ChartContext
        {
            Chart = _chartService.generateDistanceChart(TimeRes,UserSessions, SelectedYear, SelectedMonth),
            Name = "Distance",
            Unit = "m"
        };
        LitterChart = new ChartContext
        {
            Chart = _chartService.generateLitterChart(TimeRes, UserSessions),
            Name = "Litter",
            Unit = "pcs"
        };
        _isInitialized = true;
        IsBusy = false;
    }
    [RelayCommand]
    private async Task ShowMonth()
    {
        TimeRes = TimeResolution.ThisMonth;
        await Update();
    }

    [RelayCommand]
    private async Task ShowYear()
    {
        TimeRes = TimeResolution.ThisYear;
        await Update();
    }
    private async Task Update()
    {
        IsBusy = true;
        if (TimeRes is TimeResolution.ThisYear)
        {
            UserSessions.ClearAndAddRange(_allUserSessions.Where(s => s.StartDate.Year == SelectedYear));
            
        }
        else
        {
            UserSessions.ClearAndAddRange(_allUserSessions.Where(s => s.StartDate.Year == SelectedYear &&
            s.StartDate.Month == SelectedMonth));
        }
        if (!UserSessions.Any())
            await _toastService.MakeToast("No sessions found :(", CommunityToolkit.Maui.Core.ToastDuration.Short);
        LitterChart.Chart = _chartService.generateLitterChart(TimeRes, UserSessions);
        DistanceChart.Chart = _chartService.generateDistanceChart(TimeRes, UserSessions, SelectedYear, SelectedMonth);
        PloggingStats = new PloggingStatistics(UserSessions);
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
    ObservableCollection<string> months;

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
