using PlogPal.Application.Common.Interfaces;
using Location = PlogPal.Domain.Models.Location;

namespace PlogPal.Services;

public class MauiLocationProvider : ILocationProvider
{
    public async Task<Location?> GetCurrentLocation()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));

        var location = await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var location = await Geolocation.GetLocationAsync(request);
            return location;
        });

        var domainLocation = new Location(location.Latitude, location.Longitude);

        return domainLocation;
    }
}
