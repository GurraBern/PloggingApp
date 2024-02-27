using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Helpers;

namespace PloggingApp.Services.PloggingTracking;

public class PloggingSessionTracker : IPloggingSessionTracker
{
    private readonly IPloggingSessionService _ploggingSessionService;
    private List<Litter> CurrentLitter { get; set; } = [];
    private DateTime StartTime { get; set; }

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
        var ploggingData = LitterCalculator.CreatePloggingData(CurrentLitter);
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

    public void AddLitterItem(LitterType litterType, double amount)
    {
        //var weight = LitterCalculator.CalculateWeight(litterType, amount); //TODO add something similar
        var weight = 0; //TODO replace with above!
        var litter = new Litter(litterType, amount, weight);
        CurrentLitter.Add(litter);
    }
}
