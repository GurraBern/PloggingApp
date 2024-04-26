using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.MVVM.ViewModels.Popups;
using PloggingApp.Services;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel : ObservableObject, IAsyncInitialization, IRecipient<LitterPlacedMessage>, IRecipient<LitterBagPlacedMessage>, IRecipient<LitterbagPickedUpMessage>
{
    private readonly ILitterLocationService _litterLocationService;
    private readonly IUserEventService _userEventService;
    private readonly ILitterbagPlacementService _litterbagPlacementService;
    private readonly IPopupService _popupService;
    private readonly IToastService _toastService;
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];

    public Task Initialization { get; private set; }

    public MapViewModel(ILitterLocationService litterLocationService, IUserEventService userEventService, ILitterbagPlacementService litterbagPlacementService, IPopupService popupService, IToastService toastService)
    {
        _litterLocationService = litterLocationService;
        _userEventService = userEventService;
        _litterbagPlacementService = litterbagPlacementService;
        _popupService = popupService;
        _toastService = toastService;
        WeakReferenceMessenger.Default.Register<LitterPlacedMessage>(this);
        WeakReferenceMessenger.Default.Register<LitterBagPlacedMessage>(this);
        WeakReferenceMessenger.Default.Register<LitterbagPickedUpMessage>(this);

        Initialization = Initialize();
    }
    
    //TODO Reactive extensions to concurrently load these
    private async Task Initialize()
    {
        //await AddTrashPinsToMap();
        //await AddLitterBagPlacementsToMap();
        await AddEventsToMap();
    }

    private async Task AddTrashPinsToMap()
    {
        var litterLocations = await Task.Run(_litterLocationService.GetLitterLocations);

        foreach (var litterLocation in litterLocations)
        {
            PlaceTrashPin(litterLocation.Location);
        }
    }

    private async Task AddLitterBagPlacementsToMap()
    {
        var litterbagPlacements = await Task.Run(_litterbagPlacementService.GetLitterbagPlacements);

        foreach (var litterbagPlacement in litterbagPlacements)
        {
            PlaceLitterbagPin(litterbagPlacement);
        }
    }

    private async Task AddEventsToMap()
    {
        var userEvents = await _userEventService.GetEvents();
        
        foreach (var userEvent in userEvents)
        {
            PlaceEventPin(userEvent);
        }
    }

    private void PlaceTrashPin(MapPoint location)
    {
        PlacedPins.Add(new CanPin()
        {
            Label = "Litter",
            Command = PressedLitterCommand,
            Location = new Location()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude 
            }
        });
    }

    private void PlaceLitterbagPin(LitterbagPlacement litterbagPlacement)
    {
        var location = litterbagPlacement.Location;

        var litterbagPlacementPin = new LitterbagPlacementPin(PressedLitterbagPlacementCommand, litterbagPlacement)
        {
            MarkerId = litterbagPlacement.Id,
            Label = "Pickup Trashbag",
            Location = new Location()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }
        };

        PlacedPins.Add(litterbagPlacementPin);
    }

    private void PlaceEventPin(UserEvent userEvent)
    {
        var userEventPin = new UserEventPin(OpenUserEventCommand, userEvent)
        {
            Label = "Plogging Event",
            Location = new Location()
            {
                Latitude = userEvent.Location.Latitude,
                Longitude = userEvent.Location.Longitude
            }
        };

        PlacedPins.Add(userEventPin);
    }

    [RelayCommand]
    private async Task PressedLitterbagPlacement(LitterbagPlacement litterbagPlacement)
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Best);
        var currentLocation = await Geolocation.GetLocationAsync(request);

        await _popupService.ShowPopupAsync<LitterbagPlacementViewModel>(onPresenting: viewModel =>
        {
            viewModel.LitterbagPlacement = litterbagPlacement;
            viewModel.CalculateDistance(currentLocation);
        });
    }

    [RelayCommand]
    private async Task OpenUserEvent(UserEvent userEvent)
    {
        //TODO Open popup for event 
        await _popupService.ShowPopupAsync<EventPopupViewModel>();
    }

    [RelayCommand]
    private async Task PressedLitter()
    {
        await _toastService.MakeToast("Litter");
    }

    //public Location CalculateZoomOut()
    //{
    //    double Longitude = 0;
    //    double Latitude = 0;
    //    foreach(Location loc in TrackingPositions)
    //    {
    //        Longitude += loc.Longitude;
    //        Latitude += loc.Latitude;
    //    }
    //    Longitude = Longitude / TrackingPositions.Count;
    //    Latitude = Latitude / TrackingPositions.Count;
    //    Location ZoomLoc = new Location(Latitude, Longitude);
    //    return ZoomLoc;
    //}

    //public (double LatitudeRegion, double LongitudeRegion) ZoomRegion()
    //{

    //    double LatitudeMin = TrackingPositions.Min(loc => loc.Latitude);
    //    double LatitudeMax = TrackingPositions.Max(loc => loc.Latitude);
    //    double LongitudeMin = TrackingPositions.Min(loc => loc.Longitude);
    //    double LongitudeMax = TrackingPositions.Max(loc => loc.Longitude);
    //   return (LatitudeMax - LatitudeMin, LongitudeMax - LongitudeMin); 
    //}

    public void Receive(LitterPlacedMessage message)
    {
        PlaceTrashPin(message.LitterLocation);
    }

	public void Receive(LitterBagPlacedMessage message)
	{
        PlaceLitterbagPin(message.LitterbagPlacement);
	}

    public void Receive(LitterbagPickedUpMessage message)
    {
        RemoveLitterbag(message.LitterbagPlacement.Id);
    }

    //TODO better performance?
    private void RemoveLitterbag(string id)
    {
        try
        {
            var litterbagPin = PlacedPins.First(x => x.MarkerId != null && x.MarkerId.Equals(id));
            PlacedPins.Remove(litterbagPin);
        }
        catch (Exception)
        {
            _toastService.MakeToast("Could not find marker");
        }
    }
}