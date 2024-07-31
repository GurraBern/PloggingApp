using PlogPal.Application.Common.Interfaces;
using PlogPal.Domain.Enums;
using PlogPal.Domain.Models;

namespace PlogPal.Application.PloggingSessions;

public class PloggingSessionManager : IPloggingSessionManager
{
    private readonly ILocationTracker _locationTracker;
    private readonly ILitterTracker _litterTracker;

    public bool IsPlogging { get; private set; }
    private DateTime StartTime { get; set; }
    public Location CurrentLocation { get; private set; }

    private Task _updateLocation;


    public PloggingSessionManager(ILocationTracker locationTracker, ILitterTracker litterTracker)
    {
        _locationTracker = locationTracker;
        _litterTracker = litterTracker;
    }

    public void StartPlogging()
    {
        IsPlogging = true;

        StartTime = DateTime.UtcNow;

        _locationTracker.LocationUpdated += OnLocationUpdated;

        _locationTracker.TrackLocation();
    }

    private void OnLocationUpdated(object? sender, Location location)
    {
        CurrentLocation = location;
    }

    public void StopPlogging()
    {
        IsPlogging = false;

        _locationTracker.LocationUpdated -= OnLocationUpdated;


        //Ta bild
        var route = _locationTracker.PlogRoute;
        var litters = _litterTracker.Litters;

        //EF SaveAsync
    }

    public void AddLitter(LitterType litterType)
    {
        _litterTracker.AddLitter(litterType, CurrentLocation);
    }
}
