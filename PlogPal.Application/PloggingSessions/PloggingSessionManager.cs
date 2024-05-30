using PlogPal.Application.Common.Interfaces;
using PlogPal.Domain.Models;
using PlogPal.Domain.Services;

namespace PlogPal.Application.PloggingSessions;

public class PloggingSessionManager: IPloggingSessionManager
{
    private readonly ILocationProvider _locationProvider;
    private readonly RouteTracker _routeTracker;

    public bool IsPlogging { get; private set; }
    private DateTime StartTime { get; set; }
    public Location CurrentLocation { get; private set; }

    private Task _updateLocation;

    public event EventHandler<Location> LocationUpdated;

    public PloggingSessionManager(ILocationProvider locationProvider, RouteTracker routeTracker)
    {
        _locationProvider = locationProvider;
        _routeTracker = routeTracker;
    }

    public void StartPlogging()
    {
        IsPlogging = true;

        StartTime = DateTime.UtcNow;

        _updateLocation = Task.Run(UpdateLocation);
    }

    private async Task UpdateLocation()
    {
        while (IsPlogging)
        {
            CurrentLocation = await _locationProvider.GetCurrentLocation();

            if (CurrentLocation != null)
            {
                //TrackRoute();

                LocationUpdated?.Invoke(this, CurrentLocation);

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }

    public void StopPlogging()
    {
        IsPlogging = false;

    }
}
