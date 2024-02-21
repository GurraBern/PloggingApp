using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel
{
     public Polyline Polyline = new Polyline
    {
        StrokeColor = Colors.Blue,
        StrokeWidth = 15
    };
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];

    public bool isTracking = false;

    public int DISTANCE_THRESHOLD = 50;


    public MapViewModel()
    {

    }

    [RelayCommand]
    public async Task RemovePin()
    {
        if (PlacedPins.Count > 0)
        {
            int LastElement = PlacedPins.Count - 1;
            PlacedPins.RemoveAt(LastElement);
        }
    }
    [RelayCommand]
    public async Task AddTrashCollectedPin()
    {
        Location loc = await CurrentLocationAsync();
        var pin = new TrashCollectedPin()
        {
            Label = "COLLECTED",
            Location = loc,
            Address = "!!"
        };
        PlacedPins.Add(pin);
    }

    [RelayCommand]
    public async Task AddNeedHelpToCollectPin()
    {
        Location loc = await CurrentLocationAsync();
        var pin = new NeedHelpToCollectPin()
        {
            Label = "HELP",
            Location = loc,
            Address = "!!"
        };
        PlacedPins.Add(pin);
    }

    [RelayCommand]
    public async Task FinishSession()
    {
        TrackingPositions.Clear();
        PlacedPins.Clear();
        
    }

    public async Task<Location> CurrentLocationAsync()
    {
        try
        {
            var Request = new GeolocationRequest(GeolocationAccuracy.Best);
            var UpdatedLocation = await Geolocation.GetLocationAsync(Request);

            if (UpdatedLocation != null)
            {
                return UpdatedLocation;
            }
            else
            {
                // Handle case where location is null
                return null;
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            return null;
            // Handle not supported on device exception
        }
        catch (PermissionException pEx)
        {
            return null;
            // Handle permission exception
        }
    }

    [RelayCommand]
    public async Task StartTrackingLocation()
    {
        Location loc = await CurrentLocationAsync();
        TrackingPositions.Add(loc);
        var StartPin = new StartPin()
        {
            Location = loc,
            Label = "Start"
        };
        PlacedPins.Add(StartPin);
        isTracking = true;
        while (isTracking)
        {
            await KeepTracking();

        }

        loc = await CurrentLocationAsync();
        var FinishPin = new FinishPin()
        {
            Location = loc,
            Label = "End"
        };
        PlacedPins.Add(FinishPin);
        TrackingPositions.Add(loc);
        UpdatePolyline();
    }

    [RelayCommand]
    public async Task KeepTracking()
    {

        Location loc = await CurrentLocationAsync();
        if (Distance.BetweenPositions(TrackingPositions.Last(), loc).Meters > DISTANCE_THRESHOLD){ 
            TrackingPositions.Add(loc);
            //var StartPin = new StartPin()
            //{
            //    Location = loc,
            //    Label = "Start"
            //};
            //PlacedPins.Add(StartPin);
            //await Task.Delay(TimeSpan.FromSeconds(2));
        }


    }
    [RelayCommand]
    public async Task ResumeSession()
    {

        PlacedPins.RemoveAt(PlacedPins.Count - 1);
        isTracking = true;
        while (isTracking)
        {
            await KeepTracking();

        }

        Location loc = await CurrentLocationAsync();
        var FinishPin = new FinishPin()
        {
            Location = loc,
            Label = "End"
        };
        PlacedPins.Add(FinishPin);
        TrackingPositions.Add(loc);
        UpdatePolyline();
    }

    public void UpdatePolyline()
    {
       
        foreach (Location TrackingPosition in TrackingPositions)
        {
            Polyline.Geopath.Add(new Location(TrackingPosition.Latitude, TrackingPosition.Longitude));
        }
    }

    [RelayCommand]
    public async Task StopTracking()
    {
        isTracking = false;
    }








}