using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Firebase.Auth;
using Microsoft.Maui.Maps;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Services;
using PloggingApp.Services.Authentication;
using PloggingApp.Services.Camera;
using PloggingApp.Services.PloggingTracking;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class PloggingSessionViewModel : ObservableObject, IRecipient<LitterPlacedMessage>, IRecipient<PhotoTakenMessage>
{
    private readonly IPloggingSessionTracker _ploggingSessionTracker;
    private readonly ICameraService _cameraService;
    private readonly IPopupService _popupService;
    private readonly IToastService _toastService;
    private readonly IPlogTogetherService _plogTogetherService;
    private readonly IAuthenticationService _authenticationService;

    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];
    private Location CurrentLocation { get; set; }

    [ObservableProperty]
    private bool isTracking = false;

    public PloggingSessionViewModel(IPloggingSessionTracker ploggingSessionTracker, 
        ICameraService cameraService, 
        IPopupService popupService,
        IToastService toastService,
        IPlogTogetherService plogTogetherService,
        IAuthenticationService authenticationService)
    {
        _ploggingSessionTracker = ploggingSessionTracker;
        _cameraService = cameraService;
        _popupService = popupService;
        _toastService = toastService;
        _plogTogetherService = plogTogetherService;
        _authenticationService = authenticationService;

        _ploggingSessionTracker.LocationUpdated += OnLocationUpdated;

        WeakReferenceMessenger.Default.Register<LitterPlacedMessage>(this);
        WeakReferenceMessenger.Default.Register<PhotoTakenMessage>(this);

    }

    private void OnLocationUpdated(object? sender, Location location)
    {
        CurrentLocation = location;
    }

    [RelayCommand]
    private async Task StartPloggingSession()
    {
        var currentUserId = _authenticationService.CurrentUser.Uid;
        var plogGroup = await _plogTogetherService.GetPlogTogether(currentUserId);

        if (plogGroup == null)
        {
            IsTracking = true;

            _ploggingSessionTracker.StartSession();

            WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(IsTracking, []));
        }
        else
        {
            if (plogGroup.OwnerUserId == currentUserId)
            {
                IsTracking = true;

                _ploggingSessionTracker.StartSession();

                WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(IsTracking, []));
            }
            else
            {
                // plog group exists but current user is not leader, cant start session!
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Can't start session: you are not leader of the group!", "OK");
                });
            }
        }
    }

    [RelayCommand]
    private async Task EndPloggingSession()
    {
        await _popupService.ShowPopupAsync<AcceptPopupViewModel>();

        IsTracking = false;
        WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(IsTracking, []));
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
            var litterbagPlacement = new LitterbagPlacement()
            {
                Location = new MapPoint(markLocation.Latitude, markLocation.Longitude),
                PlacementDate = DateTime.UtcNow,
                ImageUrl = imagePath,
            };

            try
            {
				await _ploggingSessionTracker.AddTrashCollectionPoint(litterbagPlacement);
                WeakReferenceMessenger.Default.Send(new LitterBagPlacedMessage(litterbagPlacement));
			}
			catch (Exception ex)
            {
                await _toastService.MakeToast("Could not place litterbag request");
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
