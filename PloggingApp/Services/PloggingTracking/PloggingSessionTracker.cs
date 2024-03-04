using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;

namespace PloggingApp.Services.PloggingTracking;

public class PloggingSessionTracker : IPloggingSessionTracker
{
    private readonly IPloggingSessionService _ploggingSessionService;
    private List<Litter> CurrentLitter { get; set; } = [];
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
