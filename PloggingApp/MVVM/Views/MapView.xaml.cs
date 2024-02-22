using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class MapView : ContentView
{
    public MapView()
    {
        MoveMapToCurrentLocationAsync();
        InitializeComponent();
    }

    public void DrawPolyLine(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        Polyline Polyline = ((MapViewModel)BindingContext).Polyline;
        PloggingMap.MapElements.Add(Polyline);
        StopButton.IsVisible = false;
        Polyline.Geopath.Clear();
        ResumeButton.IsVisible = true;
        FinishButton.IsVisible = true;


    }
    public async void ResumeClick(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        StartButton.IsVisible = false;
        StopButton.IsVisible = true;
        ResumeButton.IsVisible = false;
        FinishButton.IsVisible = false;
        MapFollowFunction();
    }

 

    public void FinishClick(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        StartButton.IsVisible = true;
        StopButton.IsVisible = false;
        ResumeButton.IsVisible = false;
        FinishButton.IsVisible = false;
        MoveMapToCurrentLocationAsync();

    }


    public async void StartClick(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        StartButton.IsVisible = false;
        StopButton.IsVisible =  true;
        ResumeButton.IsVisible = false;
        MapFollowFunction();

    }
    public async Task<Location> MoveMapToCurrentLocationAsync()
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
                // Handle case where location is null
                return null;
           }
        }
        catch (FeatureNotSupportedException fnsEx)
       {
           // Handle not supported on device exception
            return null;
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            return null;
       }
        catch (Exception ex)
        {
            // Unable to get location
          return null;
        }
    }

    private async void MapFollowFunction()
    {
        while (StopButton.IsVisible)
        {
            await MoveMapToCurrentLocationAsync();
        }
        Location ZoomLoc = ((MapViewModel)BindingContext).CalculateZoomOut();
        var (Longitude, Latitude) = ((MapViewModel)BindingContext).ZoomRegion();
        MapSpan MapSpan = new MapSpan(ZoomLoc,Longitude*1.8, Latitude*1.4);
        PloggingMap.MoveToRegion(MapSpan);
    }

}