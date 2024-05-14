using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Models;
using PloggingApp.Commands;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Features.Map.Components;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Services;
using PloggingApp.Shared;
using System.Collections.ObjectModel;

namespace PloggingApp.Features.Map;

public partial class MapViewModel : ObservableObject, IAsyncInitialization, IRecipient<LitterPlacedMessage>, IRecipient<LitterBagPlacedMessage>, IRecipient<LitterbagPickedUpMessage>
{
    private readonly ILitterLocationService _litterLocationService;
    private readonly ILitterbagPlacementService _litterbagPlacementService;
    private readonly IPopupService _popupService;
    private readonly IToastService _toastService;
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];

    public Task Initialization { get; private set; }

    public MapViewModel(ILitterLocationService litterLocationService, ILitterbagPlacementService litterbagPlacementService, IPopupService popupService, IToastService toastService)
    {
        _litterLocationService = litterLocationService;
        _litterbagPlacementService = litterbagPlacementService;
        _popupService = popupService;
        _toastService = toastService;
        WeakReferenceMessenger.Default.Register<LitterPlacedMessage>(this);
        WeakReferenceMessenger.Default.Register<LitterBagPlacedMessage>(this);
        WeakReferenceMessenger.Default.Register<LitterbagPickedUpMessage>(this);

        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        await AddTrashPinsToMap();
        await AddLitterBagPlacementsToMap();
    }

    private async Task AddTrashPinsToMap()
    {
        var litterLocations = await Task.Run(_litterLocationService.GetLitterLocations);

        foreach (var litterLocation in litterLocations)
        {
            PlaceTrashPin(litterLocation.Location);
        }
    }

    private void PlaceTrashPin(MapPoint location)
    {
        PlacedPins.Add(new CollectedLitterPin()
        {
            Label = "Litter",
            Location = new Location()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude 
            }
        });
    }

    private async Task AddLitterBagPlacementsToMap()
    {
        var litterbagPlacements = await Task.Run(_litterbagPlacementService.GetLitterbagPlacements);

        foreach (var litterbagPlacement in litterbagPlacements)
        {
            PlaceLitterbag(litterbagPlacement);
        }
    }

    private void PlaceLitterbag(LitterbagPlacement litterbagPlacement)
    {
        var location = litterbagPlacement.Location;
        var openLitterbagPlacementCommand = new OpenLitterbagPlacementCommand(_popupService, litterbagPlacement);

        var litterbagPlacementPin = new LitterbagPlacementPin(openLitterbagPlacementCommand, litterbagPlacement)
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

    public void Receive(LitterPlacedMessage message)
    {
        PlaceTrashPin(message.LitterLocation);
    }

    public void Receive(LitterBagPlacedMessage message)
	{
        PlaceLitterbag(message.LitterbagPlacement);
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
