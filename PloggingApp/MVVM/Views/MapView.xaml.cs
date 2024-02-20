using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class MapView : ContentView
{
    public MapView()
    {
        MoveMapToCurrentLocationAsync();
        InitializeComponent();
    }

    public void DrawPolyLine(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
    Polyline Polyline = ((MapViewModel)BindingContext).Polyline;
        PloggingMap.MapElements.Add(Polyline);
    }
    public async Task<Location> MoveMapToCurrentLocationAsync()
            {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                PloggingMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
                return location;
            }
            else
            {
                // Handle case where location is null
                return null;
           }
        }
        catch (FeatureNotSupportedException fnsEx)
       {
           // Handle not supported on device exception
            return null;
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            return null;
       }
        catch (Exception ex)
        {
            // Unable to get location
          return null;
        }
    }

}