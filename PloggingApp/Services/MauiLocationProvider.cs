using PlogPal.Application.Common.Interfaces;

namespace PlogPal.Services;

public class MauiLocationProvider : ILocationProvider
{
    public async Task<Domain.Models.Location> GetCurrentLocation()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Best);
        var deviceLocation = await Geolocation.GetLocationAsync(request);

        var location = new Domain.Models.Location(deviceLocation.Latitude, deviceLocation.Longitude); 
        
        return location;
    }
}
