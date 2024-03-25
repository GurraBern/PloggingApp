using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Models;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;
[QueryProperty(nameof(PloggingSession), nameof(PloggingSession))]
public partial class SessionStatisticsViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    PloggingSession ploggingSession;
    public SessionStatisticsViewModel()
    {
    }

    private async void Init()
    {
        if (PloggingSession == null)
            return;
        TimeSpan = $"From {PloggingSession.StartDate.ToString("HH:mm:ss")}" + " to " +
            $"{PloggingSession.EndDate.ToString("HH:mm:ss")}";

        Area = await GetArea(PloggingSession.PloggingData.Litters.FirstOrDefault().LitterLocation.Latitude,
            PloggingSession.PloggingData.Litters.FirstOrDefault().LitterLocation.Longitude);
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
    private string timeSpan;
    [ObservableProperty]
    private string area;
}
