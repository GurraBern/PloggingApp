using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using PlogPal.Domain.Enums;
using PlogPal.Domain.Models;
using PlogPal.Maui.Services.Statistics;
using PlogPal.Maui.Shared;

namespace PloggingApp.Features.Statistics;

[QueryProperty(nameof(PlogSession), nameof(PlogSession))]
public partial class SessionStatisticsViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private PlogSession plogSession;

    private readonly IChartService _chartService;
    public SessionStatsMapViewModel SessionStatsMapViewModel { get; set; }
    public SessionStatisticsViewModel(IChartService ChartService, SessionStatsMapViewModel sessionStatsViewModel)
    {
        SessionStatsMapViewModel = sessionStatsViewModel;
        _chartService = ChartService;
    }

    private void Init()
    {
        if (PlogSession == null)
            return;
        SessionStatsMapViewModel.Initialize(PlogSession);
        GetStatistics();
   }

    private async void GetStatistics()
    {
        TimeSpan = $"{PlogSession.StartDate.ToString("HH:mm:ss")}" + " to " +
           $"{PlogSession.EndDate.ToString("HH:mm:ss")}";

        if (PlogSession.PloggingData.Litters.Any())
        {
            Area = await GetArea(PlogSession.PloggingData.Litters.First().LitterLocation);
        }
        else
        {

            Area = "?? :(";
        }

        LitterChart = _chartService.GenerateLitterChart(TimeResolution.Alltime, new List<PlogSession> { PlogSession });
        LitterWeight = PlogSession.PloggingData.Litters.Sum(x => x.Weight);
        PloggingStats = new PloggingStatistics(PlogSession);

    }

    // Using a makeshift constructor as the class constructor executes before ApplyQueryAttributes
    // which results in PlogSession being null
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        this.PlogSession = query[nameof(PlogSession)] as PlogSession;
        Init();
    }

    private async Task<string> GetArea(PlogPal.Domain.Models.Location location)
    {
        IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
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
