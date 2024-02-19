
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Lang;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel : ObservableObject
{
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];

    public MapViewModel()
    {

    }

    [RelayCommand]
    public async Task AddTrashCollectedPin()
    {
        Location loc = await updateLocationAsync();
        var pin = new TrashCollectedPin()
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
        Location loc = await updateLocationAsync();
        var pin = new NeedHelpToCollectPin()
        {
            Label = "HELP",
            Location = loc,
            Address = "!!"
        };
        PlacedPins.Add(pin);
    }

    public async Task<Location> updateLocationAsync()
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
            // Handle not supported on device exception
        }
        catch (PermissionException pEx)
        {
            return null;
            // Handle permission exception
        }
    }
}

