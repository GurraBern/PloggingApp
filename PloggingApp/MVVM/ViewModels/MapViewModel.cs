using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.Models.Messages;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel : ObservableObject, IAsyncInitialization, IRecipient<LitterPlacedMessage>
{
    private readonly ILitterLocationService _litterLocationService;
    private readonly ILitterBagPlacementService _litterBagPlacementService;

    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public Task Initialization { get; private set; }

    public MapViewModel(ILitterLocationService litterLocationService, ILitterBagPlacementService litterBagPlacementService)
    {
        _litterLocationService = litterLocationService;
        _litterBagPlacementService = litterBagPlacementService;

        WeakReferenceMessenger.Default.Register(this);

        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        await AddTrashPinsToMap();
        await AddLitterBagPlacementsToMap();
    }

    private async Task AddTrashPinsToMap()
    {
        var litterLocations = await _litterLocationService.GetLitterLocations();

        foreach (var litterLocation in litterLocations)
        {
            PlaceTrashPin(litterLocation.Location);
        }
    }

    private void PlaceTrashPin(MapPoint location)
    {
        PlacedPins.Add(new CanPin()
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
        var litterBagPlacements = await _litterBagPlacementService.GetLitterBagPlacements();

        foreach (var litterBagPlacement in litterBagPlacements)
        {
            PlaceLitterBag(litterBagPlacement);
        }
    }

    private void PlaceLitterBag(LitterBagPlacement litterBagPlacement)
    {
        var location = litterBagPlacement.Location;

        PlacedPins.Add(new LitterBagPlacementPin()
        {
            Location = new Location()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }
        });
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
}