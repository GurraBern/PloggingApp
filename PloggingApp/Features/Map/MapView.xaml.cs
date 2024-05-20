using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PloggingApp.Features.Map.Components;
using PloggingApp.Features.PloggingSession;

namespace PloggingApp.Features.Map;

public partial class MapView : ContentView, IRecipient<PloggingSessionMessage>
{
    public MapView()
    {
        InitializeComponent();

        GetLastKnownLocation().SafeFireAndForget();

        WeakReferenceMessenger.Default.Register(this);
    }

    public void DrawPolyLine(IEnumerable<Location> locations)
    {
        PloggingMap.MapElements.Clear();
        polyLine = new Polyline() 
        { 
            StrokeColor = Colors.Blue,
            StrokeWidth = 10,
        };

        foreach (var location in locations)
        {
            polyLine.Geopath.Add(location);
        }

        PloggingMap.MapElements.Add(polyLine);
    }

    private async Task MoveMapToCurrentLocationAsync()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Best);
        var location = await Geolocation.GetLocationAsync(request);

        if (location != null)
        {
            PloggingMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
        }
    }

    private async Task GetLastKnownLocation()
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null)
        {
            PloggingMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), Distance.FromMiles(1)));
        }
    }

    //Bad solution should use data binding if possible
    public async void Receive(PloggingSessionMessage message)
    {
        if (message.IsTracking)
        {
            await MoveMapToCurrentLocationAsync();
        }
        else
        {
            DrawPolyLine(message.Locations);
            MoveToRouteRegion(message.Locations);
        }
    }

    private void MoveToRouteRegion(ICollection<Location> locations)
    {
        var routeCenter = CalculateRouteCenter(locations);
        var (LatitudeRegion, LongitudeRegion) = CalculateZoomRegion(locations);
        var routeRegion = new MapSpan(routeCenter, LatitudeRegion * 1.8, LongitudeRegion * 1.4);
        PloggingMap.MoveToRegion(routeRegion);
    }

    private static Location CalculateRouteCenter(ICollection<Location> locations)
    {
        double longitude = 0;
        double latitude = 0;
        foreach (Location loc in locations)
        {
            longitude += loc.Longitude;
            latitude += loc.Latitude;
        }

        var locationCount = locations.Count;
        var zoomLocation = new Location(latitude/locationCount, longitude/locationCount);

        return zoomLocation;
    }

    private (double LatitudeRegion, double LongitudeRegion) CalculateZoomRegion(ICollection<Location> locations)
    {
        double LatitudeMin = locations.Min(loc => loc.Latitude);
        double LatitudeMax = locations.Max(loc => loc.Latitude);

        double LongitudeMin = locations.Min(loc => loc.Longitude);
        double LongitudeMax = locations.Max(loc => loc.Longitude);

        return (LatitudeMax - LatitudeMin, LongitudeMax - LongitudeMin);
    }

    private void CustomPin_MarkerClicked(object sender, PinClickedEventArgs e)
    {
        var pin = sender as CustomPin;

        var locationPin = pin?.BindingContext as LocationPin;

        locationPin?.Command?.ExecuteAsync("");
    }
}

