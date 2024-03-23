using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using Plogging.Core.Models;
using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Services.Camera;
using PloggingApp.Services.PloggingTracking;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class PloggingSessionViewModel : ObservableObject, IRecipient<LitterPlacedMessage>, IRecipient<PhotoTakenMessage>
{
    private readonly IPloggingSessionTracker _ploggingSessionTracker;
    private readonly ICameraService _cameraService;
    private readonly IPopupService _popupService;
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];
    private Location CurrentLocation { get; set; }

    [ObservableProperty]
    private bool isTracking = false;

    public PloggingSessionViewModel(IPloggingSessionTracker ploggingSessionTracker, ICameraService cameraService, IPopupService popupService)
    {
        _ploggingSessionTracker = ploggingSessionTracker;
        _cameraService = cameraService;
        _popupService = popupService;

        _ploggingSessionTracker.LocationUpdated += OnLocationUpdated;

        WeakReferenceMessenger.Default.Register<LitterPlacedMessage>(this);
        WeakReferenceMessenger.Default.Register<PhotoTakenMessage>(this);

    }

    private void OnLocationUpdated(object? sender, Location location)
    {
        CurrentLocation = location;
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
        await _popupService.ShowPopupAsync<AcceptPopupViewModel>();
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
    public void AddCanCollectedPin()
    {
        var pin = new CanPin()
        {
            Label = "COLLECTED",
            Location = CurrentLocation,
            Address = "!!"
        };

        PlacedPins.Add(pin);
    }

    [RelayCommand]
    public void AddNeedHelpToCollectPin()
    {
        var pin = new NeedHelpToCollectPin()
        {
            Label = "HELP",
            Location = CurrentLocation,
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

    [RelayCommand]
    private async Task MarkTrashForCollection()
    {
        var imagePath = await _cameraService.TakePhoto();

        var request = new GeolocationRequest(GeolocationAccuracy.Best);
        var markLocation = await Geolocation.GetLocationAsync(request);

        if(!imagePath.Equals("") && markLocation != null)
        {
            //TODO Add message for collectionpoint
            var litterBagPlacement = new LitterBagPlacement()
            {
                Location = new MapPoint(markLocation.Latitude, markLocation.Longitude),
                Image = imagePath,
                Description = "Could not recycle so I left the bag"
            };

            try
            {
				await _ploggingSessionTracker.AddTrashCollectionPoint(litterBagPlacement);
                WeakReferenceMessenger.Default.Send(new LitterBagPlacedMessage(litterBagPlacement));
			}
			catch (Exception ex)
            {
                //TODO show toast
            }
        }
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

    public void Receive(PhotoTakenMessage message)
    {
        IsTracking = false;

        var FinishPin = new FinishPin()
        {
            Location = CurrentLocation,
            Label = "End"
        };

        PlacedPins.Add(FinishPin);
        TrackingPositions.Add(CurrentLocation);

        WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(IsTracking, TrackingPositions));
    }
}
