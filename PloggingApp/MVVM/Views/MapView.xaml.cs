using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class MapView : ContentView, IRecipient<PloggingSessionMessage>
{
    public MapViewModel? MapViewModel { get; set; }

    public MapView()
    {
        InitializeComponent();

        GetLastKnownLocation().SafeFireAndForget();

        WeakReferenceMessenger.Default.Register(this);
    }

    private void Initialize()   
    {
        MapViewModel = (MapViewModel)BindingContext;
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

    private async Task<Location> MoveMapToCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                PloggingMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
                return location;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private async Task MapFollowFunction()
    {
        await MoveMapToCurrentLocationAsync();

        //Location ZoomLoc = ((MapViewModel)BindingContext).CalculateZoomOut();
        //var (Longitude, Latitude) = ((MapViewModel)BindingContext).ZoomRegion();
        //MapSpan MapSpan = new MapSpan(ZoomLoc, Longitude * 1.8, Latitude * 1.4);
        //PloggingMap.MoveToRegion(MapSpan);
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
    public void Receive(PloggingSessionMessage message)
    {
        if(MapViewModel == null)
            Initialize();

        if (message.IsTracking)
        {
            MapFollowFunction();
        }
        else
        {
            DrawPolyLine(message.Locations);
        }
    }
}

