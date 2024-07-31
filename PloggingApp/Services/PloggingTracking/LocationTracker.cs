using PlogPal.Application.Common.Interfaces;
using Location = PlogPal.Domain.Models.Location;

namespace PlogPal.Services.PloggingTracking;
public class LocationTracker : ILocationTracker
{
    private readonly ILocationProvider _locationProvider;
    public Location CurrentLocation { get; private set; }

    public event EventHandler<Location> LocationUpdated;
    public bool IsPlogging { get; set; }

    public ICollection<Location> PlogRoute { get; private set; } = [];

    public LocationTracker(ILocationProvider locationProvider)
    {
        _locationProvider = locationProvider;
    }
    
    //TODO cancelation token instead?
    public async Task TrackLocation()
    {
        IsPlogging = true;
        
        await Task.Run(UpdateLocation);
    }
    
    private async Task UpdateLocation()
    {
        while (IsPlogging)
        {
            var currentLocation = await _locationProvider.GetCurrentLocation();
            if (currentLocation == null) 
                continue;

            CurrentLocation = currentLocation;
            PlogRoute.Add(currentLocation);
                
            LocationUpdated?.Invoke(this, CurrentLocation);

            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}