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

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel
{
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];

    public bool Tracking = false;



    Microsoft.Maui.Controls.Maps.Polyline Polyline = new Microsoft.Maui.Controls.Maps.Polyline
    {
        StrokeColor = Colors.Blue,
        StrokeWidth = 12
    };

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
        Tracking = true;
        while (Tracking)
        {
            await KeepTracking();
            foreach (Location TrackingPosition in TrackingPositions)
            {
                Polyline.Geopath.Add(new Location(TrackingPosition.Latitude, TrackingPosition.Longitude));
            }
        }
    }


    public async Task KeepTracking()
    {

        Location loc = await CurrentLocationAsync();
        TrackingPositions.Add(loc);
        UpdatePolyline();
        await Task.Delay(TimeSpan.FromSeconds(15));

    }

    public void UpdatePolyline()
    {
        foreach (Location TrackingPosition in TrackingPositions)
        {
            Polyline.Geopath.Add(new Location(TrackingPosition.Latitude, TrackingPosition.Longitude));
        }
        //PloggingMap.MapElements.Add(Polyline);
    }

    [RelayCommand]
    public async Task StopTracking()
    {
        Tracking = false;
        Location loc = await CurrentLocationAsync();
        UpdatePolyline();
        var FinishPin = new FinishPin()
        {
            Location = loc,
            Label = "Start"
        };
        PlacedPins.Add(FinishPin);
    }






}