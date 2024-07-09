using PlogPal.Application.Common.Interfaces;
using PlogPal.Domain.Models;

namespace PlogPal.Application.PloggingSessions;

public class PloggingSessionManager: IPloggingSessionManager
{
    private readonly ILocationTracker _locationTracker;

    public bool IsPlogging { get; private set; }
    private DateTime StartTime { get; set; }
    public Location CurrentLocation { get; private set; }

    private Task _updateLocation;


    public PloggingSessionManager(ILocationTracker locationTracker)
    {
        _locationTracker = locationTracker;
    }

    public void StartPlogging()
    {
        IsPlogging = true;

        StartTime = DateTime.UtcNow;

        _locationTracker.TrackLocation();
    }


    public void StopPlogging()
    {
        IsPlogging = false;

    }
}
