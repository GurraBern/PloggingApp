using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Services;

namespace PloggingApp.MVVM.ViewModels;

public partial class LitterbagPlacementViewModel : ObservableObject
{
    private readonly ILitterbagPlacementService _litterbagPlacementService;
    private readonly IToastService _toastService;
    private const int LITTERBAG_DISTANCE_THRESHOLD = 20;

    [ObservableProperty]
    private LitterbagPlacement litterbagPlacement = new();

    [ObservableProperty]
    private int distanceToLitterBag = 0;

    [ObservableProperty]
    private bool canPickup = false;

    public LitterbagPlacementViewModel(ILitterbagPlacementService litterbagPlacementService, IToastService toastService)
    {
        _litterbagPlacementService = litterbagPlacementService;
        _toastService = toastService;
    }

    [RelayCommand]
    private async Task CollectLitterbag()
    {
        if(LitterbagPlacement != null && LitterbagPlacement.Id != null)
        {
            //TODO add error handling
            await _litterbagPlacementService.CollectLitterbagPlacement(LitterbagPlacement.Id, DistanceToLitterBag);

            //If successful
            WeakReferenceMessenger.Default.Send(new LitterbagPickedUpMessage(LitterbagPlacement));

            await _toastService.MakeToast("Pickup request collected successfully!");
        }
        else
        {
            await _toastService.MakeToast("No pickup request select");
        }
    }

    public void CalculateDistance(Location userLocation)
    {
        if(userLocation == null)
            return;

        var litterbagLocation = LitterbagPlacement.Location;
        var distance = Distance.BetweenPositions(new Location(litterbagLocation.Latitude, litterbagLocation.Longitude), userLocation).Meters;
        DistanceToLitterBag = (int) distance;

        if (DistanceToLitterBag < LITTERBAG_DISTANCE_THRESHOLD)
            CanPickup = true;
    }
}
