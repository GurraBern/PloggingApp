//using CommunityToolkit.Mvvm.ComponentModel;
//using SkiaSharp;
//using PloggingApp.Extensions;
//using System.Collections.ObjectModel;
//using CommunityToolkit.Mvvm.Input;
//using PloggingApp.Services.Authentication;
//using PloggingApp.Shared;
//using PlogPal.Domain.Enums;
//using PlogPal.Domain.Models;
//using PlogPal.Maui.Services.Statistics;

//namespace PloggingApp.Features.Statistics;

//public partial class StatisticsViewModel : BaseViewModel, IAsyncInitialization
//{
//    private readonly IPloggingSessionService _ploggingSessionService;
//    private readonly IAuthenticationService _authenticationService;
//    private readonly IChartService _chartService;
//    private readonly IToastService _toastService;
//    public ObservableCollection<PlogSession> UserSessions { get; set; } = [];
//    private IEnumerable<PlogSession> _allUserSessions = [];
//    private readonly Dictionary<TimeResolution, string> colorDict = new()
//    {
//        {TimeResolution.ThisYear,"#5c5aa8" },
//        {TimeResolution.ThisMonth, "#9558a8"}
//    };
//    private bool _isInitialized = false;
//    private int _selectedYear;
//    private int _selectedMonth;
//    public int SelectedYear
//    {
//        get => _selectedYear;
//        set
//        {
//            _selectedYear = value;
            
//            if (!_isInitialized)
//                return;
//            Update();
//            OnPropertyChanged(nameof(SelectedYear));
//        }
//    }
//    public int SelectedMonth
//    {
//        get => _selectedMonth;
//        set
//        {
//           _selectedMonth = value;
//           if (!_isInitialized)
//                return;
//           OnPropertyChanged(nameof(SelectedMonth));
//           Update();
//        }
//    }

//    public Task Initialization { get; private set; }


//    public StatisticsViewModel(IPloggingSessionService ploggingSessionService, IAuthenticationService authenticationService, IChartService chartService, IToastService toastService)
//    {
//        _ploggingSessionService = ploggingSessionService;
//        _authenticationService = authenticationService;
//        _chartService = chartService;
//        _toastService = toastService;
        
//        Years = new ObservableCollection<int>(Enumerable.Range(DateTime.UtcNow.Year - 2, 3));
//        Months = new ObservableCollection<string>(Enum.GetNames(typeof(Month)).ToList());
//        TimeRes = TimeResolution.ThisYear;
//        StatsBoxColor = colorDict[TimeRes];
//        SelectedYear = DateTime.UtcNow.Year;
//        SelectedMonth = DateTime.UtcNow.Month - 1;
//        Initialization = InitializeAsync();
//    }
//    private async Task InitializeAsync()
//    {
//        await GetUserSessions();
//    }
//    private async Task GetUserSessions()
//    {
//        IsBusy = true;
//        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.UserId, DateTime.UtcNow.AddYears(-3), DateTime.UtcNow);
//        if (!_allUserSessions.Any())
//            await _toastService.MakeToast("No sessions found :(", CommunityToolkit.Maui.Core.ToastDuration.Short);
//        UserSessions.ClearAndAddRange(_allUserSessions);
//        PloggingStats = new PloggingStatistics(UserSessions);
//        DistanceChart = new ChartContext
//        {
//            Name = "Distance",
//            Unit = "m",
//            Color = SKColor.Parse("#3bac7c"),
//            ImageURI = "distance.svg"
//        };
//        WeightChart = new ChartContext
//        {
//            Name = "Litter Weight",
//            Unit = "kg",
//            Color = SKColor.Parse("#3b84ac"),
//            ImageURI = "scale.svg" 
//        };
//        LitterChart = new ChartContext
//        {
//            Name = "Litter",
//            Unit = "pcs",
//            ImageURI = "trashcan.svg"
//        };
//        TimeChart = new ChartContext
//        {
//            Name = "Time Plogged",
//            Unit = "minutes",
//            Color = SKColor.Parse("#ac833b"),
//            ImageURI = "clock.svg"
//        };
//        Co2savedChart = new ChartContext
//        {
//            Name = "CO2e Saved",
//            Unit = "kg CO2e",
//            Color = SKColor.Parse("#ac3b7f"),
//            ImageURI = "leaf.svg" 
//        };
        
//        GetCharts();
//        Charts = new ObservableCollection<ChartContext>()
//        {
//            DistanceChart, TimeChart, WeightChart, Co2savedChart
//        };
//        _isInitialized = true;
//        IsBusy = false;
//    }
//    [RelayCommand]
//    private async Task ShowMonth()
//    {
//        TimeRes = TimeResolution.ThisMonth;
//        await Update();
//    }

//    [RelayCommand]
//    private async Task ShowYear()
//    {
//        TimeRes = TimeResolution.ThisYear;
//        await Update();
//    }
//    private async Task Update()
//    {
//        IsBusy = true;
//        if (TimeRes is TimeResolution.ThisYear)
//        {
//            UserSessions.ClearAndAddRange(_allUserSessions.Where(s => s.StartDate.Year == SelectedYear));
            
//        }
//        else
//        {
//            UserSessions.ClearAndAddRange(_allUserSessions.Where(s => s.StartDate.Year == SelectedYear &&
//            s.StartDate.Month == SelectedMonth + 1));
//        }
//        if (!UserSessions.Any())
//            await _toastService.MakeToast("No sessions found :(", CommunityToolkit.Maui.Core.ToastDuration.Short);
//        GetCharts(); 
//        PloggingStats = new PloggingStatistics(UserSessions);
//        StatsBoxColor = colorDict[TimeRes];
//        IsBusy = false;
//    }
    
//    private void GetCharts()
//    {
//        LitterChart.Chart = _chartService.GenerateLitterChart(TimeRes, UserSessions);
//        DistanceChart.Chart = _chartService.GenerateLineChart(TimeRes, UserSessions, s => s.PloggingData.Distance, DistanceChart.Color, SelectedYear, SelectedMonth + 1);
//        WeightChart.Chart = _chartService.GenerateLineChart(TimeRes, UserSessions, s => s.PloggingData.Weight, WeightChart.Color, SelectedYear, SelectedMonth + 1);
//        TimeChart.Chart = _chartService.GenerateLineChart(TimeRes, UserSessions, s => (s.EndDate - s.StartDate).TotalMinutes, TimeChart.Color, SelectedYear, SelectedMonth + 1);
//        Co2savedChart.Chart = _chartService.generateLineChart(TimeRes, UserSessions, s => CO2SavedCalculator.CalculateCO2Saved(s), Co2savedChart.Color, SelectedYear, SelectedMonth + 1);
//    }
//    [RelayCommand]
//    private async Task GoToSessionStats(PlogSession session)
//    {
//        if (session is null)
//            return;
//        await Shell.Current.GoToAsync($"{nameof(SessionStatisticsPage)}", true, 
//            new Dictionary<string, object>
//            {
//                {nameof(PlogSession), session}
//            });
//    }

//    [RelayCommand]
//    private async Task Refresh()
//    {
//        IsBusy = true;
//        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.UserId, DateTime.UtcNow.AddYears(-3), DateTime.UtcNow);
//        Update();
//        IsRefreshing = false;
//        IsBusy = false;
//    }

//    [ObservableProperty]
//    ObservableCollection<string> months;

//    [ObservableProperty]
//    ObservableCollection<int> years;

//    [ObservableProperty]
//    int filterYear;

//    [ObservableProperty]
//    int filterMonth;

//    [ObservableProperty]
//    bool isRefreshing;

//    [ObservableProperty]
//    ChartContext distanceChart;

//    [ObservableProperty]
//    ChartContext weightChart;

//    [ObservableProperty]
//    ChartContext litterChart;

//    [ObservableProperty]
//    ChartContext timeChart;

//    [ObservableProperty]
//    ChartContext co2savedChart;

//    [ObservableProperty]
//    ObservableCollection<ChartContext> charts;

//    [ObservableProperty]
//    TimeResolution timeRes;

//    [ObservableProperty]
//    PloggingStatistics ploggingStats;

//    [ObservableProperty]
//    string statsBoxColor;
//}
