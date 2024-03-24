using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Maps;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;

namespace PloggingApp.MVVM.ViewModels;

public partial class LitterbagPlacementViewModel : ObservableObject
{
    private readonly ILitterbagPlacementService _litterbagPlacementService;
    private const int LITTERBAG_DISTANCE_THRESHOLD = 40;

    [ObservableProperty]
    private LitterbagPlacement litterbagPlacement = new();

    [ObservableProperty]
    private int distanceToLitterBag = 0;

    [ObservableProperty]
    private bool canPickup = false;

    public LitterbagPlacementViewModel(ILitterbagPlacementService litterbagPlacementService)
    {
        _litterbagPlacementService = litterbagPlacementService;
    }

    [RelayCommand]
    private async Task CollectLitterbag()
    {
        await _litterbagPlacementService.CollectLitterbagPlacement(litterbagPlacement.Id, DistanceToLitterBag);
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
