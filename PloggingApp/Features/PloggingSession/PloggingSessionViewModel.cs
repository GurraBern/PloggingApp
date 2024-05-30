//using CommunityToolkit.Maui.Core;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using CommunityToolkit.Mvvm.Messaging;
//using Microsoft.Maui.Maps;
//using PloggingApp.Features.LitterPickupRequests;
//using PloggingApp.Features.Map.Components;
//using PloggingApp.Features.Plogtogether;
//using PloggingApp.Services.Camera;
//using PloggingApp.Services.PloggingTracking;
//using PloggingApp.Shared;
//using PlogPal.Application.Common.Interfaces;
//using PlogPal.Domain.Models;
//using System.Collections.ObjectModel;

//namespace PloggingApp.Features.PloggingSession;

//public partial class PloggingSessionViewModel : ObservableObject, IRecipient<LitterPlacedMessage>, IRecipient<PhotoTakenMessage>
//{
//    private readonly IPloggingSessionManager _ploggingSessionManager;
//    //private readonly IPloggingSessionTracker _ploggingSessionTracker;
//    private readonly ICameraService _cameraService;
//    private readonly IPopupService _popupService;
//    private readonly IToastService _toastService;
//    //private readonly IPlogTogetherService _plogTogetherService;
//    //private readonly IAuthenticationService _authenticationService;

//    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
//    public List<Microsoft.Maui.Devices.Sensors.Location> TrackingPositions { get; set; } = [];
//    private Microsoft.Maui.Devices.Sensors.Location CurrentLocation { get; set; }

//    [ObservableProperty]
//    private bool isTracking = false;

//    public PloggingSessionViewModel(
//        IPloggingSessionManager ploggingSessionManager,
//        IPloggingSessionTracker ploggingSessionTracker, 
//        ICameraService cameraService, 
//        IPopupService popupService,
//        IToastService toastService,
//        IPlogTogetherService plogTogetherService,
//        IAuthenticationService authenticationService)
//    {
//        _ploggingSessionManager = ploggingSessionManager;
//        _ploggingSessionTracker = ploggingSessionTracker;
//        _cameraService = cameraService;
//        _popupService = popupService;
//        _toastService = toastService;
//        _plogTogetherService = plogTogetherService;
//        _authenticationService = authenticationService;

//        _ploggingSessionTracker.LocationUpdated += OnLocationUpdated;

//        WeakReferenceMessenger.Default.Register<LitterPlacedMessage>(this);
//        WeakReferenceMessenger.Default.Register<PhotoTakenMessage>(this);
//    }

//    private void OnLocationUpdated(object? sender, Microsoft.Maui.Devices.Sensors.Location location)
//    {
//        CurrentLocation = location;
//    }

//    [RelayCommand]
//    private void StartPloggingSession()
//    {
//        _ploggingSessionManager.StartPlogging();
//    }

//    [RelayCommand]
//    private async Task EndPloggingSession()
//    {
//        var imagePath = await _cameraService.TakePhoto();
//        await Shell.Current.GoToAsync($"{nameof(CheckoutImagePage)}?ImagePath={imagePath}");

//        IsTracking = false;
//        WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(IsTracking, TrackingPositions));
//    }

//    [RelayCommand]
//    private async Task StartPloggTogether()
//    {
//        await Shell.Current.GoToAsync($"{nameof(PlogTogetherPage)}?UserId={_authenticationService.UserId}");
//    }

//    [RelayCommand]
//    public void RemovePin()
//    {
//        int LastElement = PlacedPins.Count - 1;
//        if (PlacedPins.ElementAt(LastElement).GetType() != typeof(StartPin) && PlacedPins.ElementAt(LastElement).GetType() != typeof(FinishPin))
//        {
//            PlacedPins.RemoveAt(LastElement);
//        }
//    }

//    [RelayCommand]
//    public void AddCanCollectedPin()
//    {
//        var pin = new CollectedLitterPin()
//        {
//            Label = "COLLECTED",
//            Location = CurrentLocation,
//            Address = "!!"
//        };

//        PlacedPins.Add(pin);
//    }

//    [RelayCommand]
//    public void FinishSession()
//    {
//        TrackingPositions.Clear();
//        PlacedPins.Clear();
//    }

//    [RelayCommand]
//    private async Task MarkTrashForCollection()
//    {
//        var imagePath = await _cameraService.TakePhoto();

//        var request = new GeolocationRequest(GeolocationAccuracy.Best);
//        var markLocation = await Geolocation.GetLocationAsync(request);

//        if(!imagePath.Equals("") && markLocation != null)
//        {
//            var litterbagPlacement = new LitterbagPlacement()
//            {
//                Location = new PlogPal.Domain.Models.Location(markLocation.Latitude, markLocation.Longitude),
//                PlacementDate = DateTime.UtcNow,
//                ImageUrl = imagePath,
//            };

//            try
//            {
//				await _ploggingSessionTracker.AddTrashCollectionPoint(litterbagPlacement);
//                WeakReferenceMessenger.Default.Send(new LitterBagPlacedMessage(litterbagPlacement));
//			}
//			catch (Exception ex)
//            {
//                await _toastService.MakeToast("Could not place litterbag request");
//            }

//            await _toastService.MakeToast("Placed pickup request successfully!");
//        }
//    }

//    private double CalculateTotalDistance()
//    {
//        double totalDistance = 0;

//        for (int i = 0; i < TrackingPositions.Count - 1; i++)
//        {
//            double distance = Distance.BetweenPositions(TrackingPositions.ElementAt(i), TrackingPositions.ElementAt(i + 1)).Meters;
//            totalDistance += distance;
//        }

//        return totalDistance;
//    }

//    private int CalculateAverageSteps()
//    {
//        int Steps;

//        Steps = (int)Math.Floor(CalculateTotalDistance() / 1000 * 1350);

//        return Steps;
//    }


//    public void Receive(LitterPlacedMessage message)
//    {
//        var location = message.LitterLocation;
//        TrackingPositions.Add(new Microsoft.Maui.Devices.Sensors.Location(location.Latitude, location.Longitude));
//    }

//    public void Receive(PhotoTakenMessage message)
//    {
//        IsTracking = false;

//        var FinishPin = new FinishPin()
//        {
//            Location = CurrentLocation,
//            Label = "End"
//        };

//        PlacedPins.Add(FinishPin);
//    }
//}
