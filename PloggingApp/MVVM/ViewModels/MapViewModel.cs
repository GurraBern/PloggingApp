using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.Models.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel : ObservableObject, IAsyncInitialization, IRecipient<LitterPlacedMessage>
{
    private readonly ILitterLocationService _litterLocationService;
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];
    public List<Location> TrackingPositions { get; set; } = [];
    public int DISTANCE_THRESHOLD = 50;
    
    public Task Initialization { get; private set; }

    public MapViewModel(ILitterLocationService litterLocationService)
    {
        _litterLocationService = litterLocationService;

        WeakReferenceMessenger.Default.Register(this);

        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        await AddTrashPinsToMap();
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
        PlacedPins.Add(new TrashCollectedPin()
        {
            Label = "Litter",
            Location = new Location()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude 
            }
        });
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

    public void Receive(LitterPlacedMessage message)
    {
        PlaceTrashPin(message.LitterLocation);
    }
}