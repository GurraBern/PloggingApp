using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Helpers;

namespace PloggingApp.Services.PloggingTracking;

public class PloggingSessionTracker : IPloggingSessionTracker
{
    private readonly IPloggingSessionService _ploggingSessionService;
    private List<Litter> CurrentLitter { get; set; } = [];
    private List<Location> LitterLocations { get; set; } = [];
    private DateTime StartTime { get; set; }
    public Location CurrentLocation { get; set; }

    public PloggingSessionTracker(IPloggingSessionService ploggingSessionService)
    {
        _ploggingSessionService = ploggingSessionService;
    }

    public void StartSession()
    {
        StartTime = DateTime.UtcNow;
    }

    public async Task EndSession()
    {
        var ploggingData = LitterCalculator.CreatePloggingData(CurrentLitter, LitterLocations);
        var ploggingSession = new PloggingSession()
        {
            UserId = "TODOsetUserId",
            DisplayName = "TODOsetDisplayName",
            StartDate = StartTime,
            EndDate = DateTime.UtcNow,
            PloggingData = ploggingData
        };

        await _ploggingSessionService.SavePloggingSession(ploggingSession);
    }

    public void AddLitterItem(LitterType litterType, double amount, Location location)
    {
        if (location == null)
            return; //TODO show toast that not able to 
        //var weight = LitterCalculator.CalculateWeight(litterType, amount); //TODO add something similar
        var weight = 0; //TODO replace with above!
        var litter = new Litter(litterType, amount, weight);
        CurrentLitter.Add(litter);
        LitterLocations.Add(new Location(location.Longitude, location.Latitude));
    }
}
