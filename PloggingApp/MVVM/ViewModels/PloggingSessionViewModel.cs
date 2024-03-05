using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Services.PloggingTracking;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class PloggingSessionViewModel : ObservableObject, IRecipient<LitterPlacedMessage>
{
    private readonly IPloggingSessionTracker _ploggingSessionTracker;
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];

    [ObservableProperty]
    private bool isTracking = false;

    public PloggingSessionViewModel(IPloggingSessionTracker ploggingSessionTracker)
    {
        _ploggingSessionTracker = ploggingSessionTracker;

        WeakReferenceMessenger.Default.Register(this);
    }

    [RelayCommand]
    private void StartPloggingSession()
    {
        IsTracking = true;

        _ploggingSessionTracker.StartSession();

        WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(IsTracking, []));
    }

    [RelayCommand]
    private async Task EndPloggingSession()
    {
        IsTracking = false;

        var FinishPin = new FinishPin()
        {
            Location = _ploggingSessionTracker.CurrentLocation,
            Label = "End"
        };

        PlacedPins.Add(FinishPin);
        TrackingPositions.Add(_ploggingSessionTracker.CurrentLocation);

        WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(IsTracking, TrackingPositions));
        
        await _ploggingSessionTracker.EndSession();
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
        Location location = await CurrentLocationAsync();
        var pin = new NeedHelpToCollectPin()
        {
            Label = "HELP",
            Location = location,
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

    [RelayCommand]
    public void StopTracking()
    {
        IsTracking = false;
    }

    public Location CalculateZoomOut()
    {
        double longitude = 0;
        double latitude = 0;
        foreach (Location loc in TrackingPositions)
        {
            longitude += loc.Longitude;
            latitude += loc.Latitude;
        }
        longitude = longitude / TrackingPositions.Count;
        latitude = latitude / TrackingPositions.Count;
        Location ZoomLoc = new Location(latitude, longitude);
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

    public void Receive(LitterPlacedMessage message)
    {
        var location = message.LitterLocation;
        TrackingPositions.Add(new Location(location.Latitude, location.Longitude));
    }
}
