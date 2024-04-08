using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.Services.Statistics;

namespace PloggingApp.Pages;
[QueryProperty(nameof(PloggingSession), nameof(PloggingSession))]
public partial class SessionStatisticsViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    PloggingSession ploggingSession;

    private IChartService chartService;
    public SessionStatsMapViewModel SessionStatsMapViewModel { get; set; }
    public SessionStatisticsViewModel(IChartService ChartService, SessionStatsMapViewModel sessionStatsViewModel)
    {
        SessionStatsMapViewModel = sessionStatsViewModel;
        this.chartService = ChartService;
    }

    private void Init()
    {
        if (PloggingSession == null)
            return;
        SessionStatsMapViewModel.Initialize(ploggingSession);
        getStatistics();
   }

    private async void getStatistics()
    {
        TimeSpan = $"From {PloggingSession.StartDate.ToString("HH:mm:ss")}" + " to " +
           $"{PloggingSession.EndDate.ToString("HH:mm:ss")}";

        Area = await GetArea(PloggingSession.PloggingData.Litters.FirstOrDefault().LitterLocation.Latitude,
            PloggingSession.PloggingData.Litters.FirstOrDefault().LitterLocation.Longitude);

        LitterChart = chartService.generateLitterChart(TimeResolution.Alltime, new List<PloggingSession> { PloggingSession });
        LitterWeight = PloggingSession.PloggingData.Litters.Sum(x => x.Weight);
        PloggingStats = new PloggingStatistics(PloggingSession);

    }

    // Using a makeshift constructor as the class constructor executes before ApplyQueryAttributes
    // which results in PloggingSession being null
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        this.PloggingSession = query[nameof(PloggingSession)] as PloggingSession;
        Init();
    }

    private async Task<string> GetArea(double latitude, double longitude)
    {
        IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
        Placemark placemark = placemarks?.FirstOrDefault();
        if (placemark != null)
            return $"{placemark.AdminArea}, {placemark.SubLocality}";
        return "N/A";
    }

    [ObservableProperty]
    PloggingStatistics ploggingStats;
    [ObservableProperty]
    Chart litterChart;
    [ObservableProperty]
    private string timeSpan;
    [ObservableProperty]
    private string area;
    [ObservableProperty]
    private double litterWeight;
}
