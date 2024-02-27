using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Plogging.Core.Models;
using PloggingApp.MVVM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel
{
    // public Polyline Polyline = new Polyline
    //{
    //    StrokeColor = Colors.Blue,
    //    StrokeWidth = 15
    //};
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];

    public bool isTracking = false;

    public int DISTANCE_THRESHOLD = 50;

    DateTime StartTime;


    public MapViewModel()
    {
        
    }

    //[RelayCommand]
    //public void RemovePin()
    //{
    //    int LastElement = PlacedPins.Count - 1;
    //    if (PlacedPins.ElementAt(LastElement).GetType() != typeof(StartPin) && PlacedPins.ElementAt(LastElement).GetType() != typeof(FinishPin))
    //    {

    //        PlacedPins.RemoveAt(LastElement);
    //    }

    //}
    //[RelayCommand]
    //public async Task AddCanCollectedPin()
    //{
    //    Location loc = await CurrentLocationAsync();
    //    var pin = new CanPin()
    //    {
    //        Label = "COLLECTED",
    //        Location = loc,
    //        Address = "!!"
    //    };
    //    PlacedPins.Add(pin);
    //}

    //[RelayCommand]
    //public async Task AddNeedHelpToCollectPin()
    //{
    //    Location loc = await CurrentLocationAsync();
    //    var pin = new NeedHelpToCollectPin()
    //    {
    //        Label = "HELP",
    //        Location = loc,
    //        Address = "!!"
    //    };
    //    PlacedPins.Add(pin);
    //}

    //[RelayCommand]
    //public void FinishSession()
    //{

    //    PloggingData PData = new PloggingData
    //    {
    //        ScrapCount = PlacedPins.Count-2,
    //        Distance = CalculateTotalDistance(),
    //        Steps = CalculateAverageSteps(),
    //    };

    //    PloggingSession PSession = new PloggingSession {
    //        UserId ="",
    //        DisplayName ="",
    //        Id = "",
    //        StartDate = StartTime,
    //        EndDate = DateTime.Now,
    //        PloggingData = PData
    //    };

    //TrackingPositions.Clear();
    //    PlacedPins.Clear();


    //}

    //private double CalculateTotalDistance()
    //{
    //    double totalDistance = 0;

    //    for (int i = 0; i < TrackingPositions.Count - 1; i++)
    //    {
    //        double distance = Distance.BetweenPositions(TrackingPositions.ElementAt(i), TrackingPositions.ElementAt(i + 1)).Meters;
    //        totalDistance += distance;
    //    }

    //    return totalDistance;
    //}

    //private int CalculateAverageSteps()
    //{
    //    int Steps;

    //    Steps = (int)Math.Floor(CalculateTotalDistance() / 1000 * 1350);

    //    return Steps;
    //}



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

    //[RelayCommand]
    //public async Task StartTrackingLocation()
    //{
    //    StartTime = DateTime.Now;
    //    Location loc = await CurrentLocationAsync();
    //    TrackingPositions.Add(loc);
    //    var StartPin = new StartPin()
    //    {
    //        Location = loc,
    //        Label = "Start"
    //    };
    //    PlacedPins.Add(StartPin);
    //    isTracking = true;
    //    while (isTracking)
    //    {
    //        await KeepTracking();

    //    }

    //    loc = await CurrentLocationAsync();
    //    var FinishPin = new FinishPin()
    //    {
    //        Location = loc,
    //        Label = "End"
    //    };
    //    PlacedPins.Add(FinishPin);
    //    TrackingPositions.Add(loc);
    //    UpdatePolyline();
    //}

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
       
        //foreach (Location TrackingPosition in TrackingPositions)
        //{
        //    Polyline.Geopath.Add(new Location(TrackingPosition.Latitude, TrackingPosition.Longitude));
        //}
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
        foreach(Location loc in TrackingPositions)
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