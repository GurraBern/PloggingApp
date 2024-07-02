using Location = PlogPal.Domain.Models.Location;

namespace PlogPal.Maui.Extensions;

public static class LocationExtensions
{
    public static Microsoft.Maui.Devices.Sensors.Location ToExternalLocation(this Location location)
    {
        var externalLocation = new Microsoft.Maui.Devices.Sensors.Location();
        externalLocation.Longitude = location.Longitude;
        externalLocation.Latitude = location.Latitude;
        
        return externalLocation;
    }
}