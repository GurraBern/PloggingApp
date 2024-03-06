using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;

namespace PloggingApp.Services.PloggingTracking;

public class PloggingSessionTracker : IPloggingSessionTracker
{
    private readonly IPloggingSessionService _ploggingSessionService;
    private const int DISTANCE_THRESHOLD = 20;
    private Task _updateSession;
    private List<Litter> CurrentLitter { get; set; } = [];
    private DateTime StartTime { get; set; }
    public Location CurrentLocation { get; set; }
    public bool IsTracking { get; set; }
    public event EventHandler<Location> LocationUpdated;

    public PloggingSessionTracker(IPloggingSessionService ploggingSessionService)
    {
        _ploggingSessionService = ploggingSessionService;
    }

    public void StartSession()
    {
        IsTracking = true;
        StartTime = DateTime.UtcNow;

        _updateSession = Task.Run(UpdateSession);
    }

    private async Task UpdateSession()
    {
        //TODO add distance
        while(IsTracking)
        {
            CurrentLocation = await CurrentLocationAsync();

            LocationUpdated?.Invoke(this, CurrentLocation);

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public async Task<Location> CurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var updatedLocation = await Geolocation.GetLocationAsync(request);

            if(updatedLocation != null)
            {
                return updatedLocation;
            } 
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task EndSession()
    {
        IsTracking = false;

        var ploggingSession = new PloggingSession()
        {
            UserId = "TODOsetUserId",
            DisplayName = "TODOsetDisplayName",
            StartDate = StartTime,
            EndDate = DateTime.UtcNow,
            PloggingData = new PloggingData()
            {
                Litters = CurrentLitter
            } 
        };



        await _ploggingSessionService.SavePloggingSession(ploggingSession);
    }

    public void AddLitterItem(LitterType litterType, double amount, Location location)
    {
        if (location == null)
            return; //TODO show toast that not able to 

        //var weight = LitterCalculator.CalculateWeight(litterType, amount); //TODO add something similar
        var weight = 1; //TODO replace with above!

        var litterLocation = new MapPoint(location.Latitude, location.Longitude);
        var litter = new Litter(litterType, amount, litterLocation, weight);
        CurrentLitter.Add(litter);

    }
}
