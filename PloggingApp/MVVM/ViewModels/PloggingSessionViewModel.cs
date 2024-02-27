using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Plogging.Core.Models;
using PloggingApp.MVVM.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class PloggingSessionViewModel: ObservableObject
{
    public Polyline Polyline = new Polyline
    {
        StrokeColor = Colors.Blue,
        StrokeWidth = 15
    };
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];

    [ObservableProperty]
    private bool isTracking = false;

    private const int DISTANCE_THRESHOLD = 50;


    public PloggingSessionViewModel()
    {
        
    }

    [RelayCommand]
    private async Task StartPloggingSession()
    {
        IsTracking = true;

        await StartTrackingLocation();

        WeakReferenceMessenger.Default.Send(this);

        //TODO notify listeners that StartPlogging is running
        //Clear map
        //Follow person

        //PloggingMap.MapElements.Clear();
        ////InSessionButtons();
        //MapFollowFunction();   
    }

    [RelayCommand]
    private async Task EndPloggingSession()
    {
        IsTracking = false;

        var currentLocation = await CurrentLocationAsync();
        var FinishPin = new FinishPin()
        {
            Location = currentLocation,
            Label = "End"
        };
        
        PlacedPins.Add(FinishPin);
        TrackingPositions.Add(currentLocation);
        UpdatePolyline();
    }

    private async Task StartTrackingLocation()
    {
        Location currentLocation = await CurrentLocationAsync();
        TrackingPositions.Add(currentLocation);
        var StartPin = new StartPin()
        {
            Location = currentLocation,
            Label = "Start"
        };

        PlacedPins.Add(StartPin);

        while (IsTracking)
        {
            await KeepTracking();
        }

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
        }
        catch (PermissionException pEx)
        {
            return null;
        }
    }

    [RelayCommand]
    public void RemovePin()
    {
        int LastElement = PlacedPins.Count - 1;
        if (PlacedPins.ElementAt(LastElement).GetType() != typeof(StartPin) && PlacedPins.ElementAt(LastElement).GetType() != typeof(FinishPin))
        {

            PlacedPins.RemoveAt(LastElement);
        }

    }
    [RelayCommand]
    public async Task AddCanCollectedPin()
    {
        Location loc = await CurrentLocationAsync();
        var pin = new CanPin()
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
    public void FinishSession()
    {
        TrackingPositions.Clear();
        PlacedPins.Clear();
    }

    private double CalculateTotalDistance()
    {
        double totalDistance = 0;

        for (int i = 0; i < TrackingPositions.Count - 1; i++)
        {
            double distance = Distance.BetweenPositions(TrackingPositions.ElementAt(i), TrackingPositions.ElementAt(i + 1)).Meters;
            totalDistance += distance;
        }

        return totalDistance;
    }

    private int CalculateAverageSteps()
    {
        int Steps;

        Steps = (int)Math.Floor(CalculateTotalDistance() / 1000 * 1350);

        return Steps;
    }

    public async Task KeepTracking()
    {

        Location currentLocation = await CurrentLocationAsync();
        if (Distance.BetweenPositions(TrackingPositions.Last(), currentLocation).Meters > DISTANCE_THRESHOLD)
        {
            TrackingPositions.Add(currentLocation);
            //var StartPin = new StartPin()
            //{
            //    Location = loc,
            //    Label = "Start"
            //};
            //PlacedPins.Add(StartPin);
            //await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }

    //[RelayCommand]
    //public async Task ResumeSession()
    //{

    //    PlacedPins.RemoveAt(PlacedPins.Count - 1);
    //    IsTracking = true;
    //    while (isTracking)
    //    {
    //        await KeepTracking();

    //    }

    //    Location loc = await CurrentLocationAsync();
    //    var FinishPin = new FinishPin()
    //    {
    //        Location = loc,
    //        Label = "End"
    //    };
    //    PlacedPins.Add(FinishPin);
    //    TrackingPositions.Add(loc);
    //    UpdatePolyline();
    //}

    public void UpdatePolyline()
    {

        foreach (Location TrackingPosition in TrackingPositions)
        {
            Polyline.Geopath.Add(new Location(TrackingPosition.Latitude, TrackingPosition.Longitude));
        }
    }

    [RelayCommand]
    public void StopTracking()
    {
        isTracking = false;
    }

    public Location CalculateZoomOut()
    {
        double Longitude = 0;
        double Latitude = 0;
        foreach (Location loc in TrackingPositions)
        {
            Longitude += loc.Longitude;
            Latitude += loc.Latitude;
        }
        Longitude = Longitude / TrackingPositions.Count;
        Latitude = Latitude / TrackingPositions.Count;
        Location ZoomLoc = new Location(Latitude, Longitude);
        return ZoomLoc;
    }

    public (double LatitudeRegion, double LongitudeRegion) ZoomRegion()
    {

        double LatitudeMin = TrackingPositions.Min(loc => loc.Latitude);
        double LatitudeMax = TrackingPositions.Max(loc => loc.Latitude);
        double LongitudeMin = TrackingPositions.Min(loc => loc.Longitude);
        double LongitudeMax = TrackingPositions.Max(loc => loc.Longitude);
        return (LatitudeMax - LatitudeMin, LongitudeMax - LongitudeMin);

    }

    private double DistanceCalc(Location loc1, Location loc2)
    {
        double distance = Distance.BetweenPositions(loc1, loc2).Meters;
        return distance;
    }
}
