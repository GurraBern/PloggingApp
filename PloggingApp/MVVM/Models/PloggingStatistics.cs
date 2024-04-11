﻿using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models;
public partial class PloggingStatistics : ObservableObject
{
    private bool isSingleSession { get; set; } = true;
    private TimeResolution currentTR {get; set;}

    [ObservableProperty]
    public double totalSteps;
    [ObservableProperty]
    public double totalDistance;
    [ObservableProperty]
    public double totalCO2Saved;
    [ObservableProperty]
    public double totalWeight;
    [ObservableProperty]
    public TimeSpan totalTime;

    public PloggingStatistics(IEnumerable<PloggingSession> sessions)
    {
        isSingleSession = false;
        TotalSteps = sessions.Sum(s => s.PloggingData.Steps);
        TotalDistance = Math.Round(sessions.Sum(s => s.PloggingData.Distance), 1);
        TotalCO2Saved = 0;
        TotalWeight = Math.Round(sessions.Sum(s => s.PloggingData.Litters.Sum(l => l.Weight)), 1);
        TotalTime = calculateTime(sessions);
    }
    public PloggingStatistics(PloggingSession session)
    {
        TotalSteps = session.PloggingData.Steps;
        TotalDistance = Math.Round(session.PloggingData.Distance, 2);
        TotalWeight = Math.Round(session.PloggingData.Litters.Sum(s => s.Weight), 2);
        TotalTime = session.EndDate - session.StartDate;
    }
    private TimeSpan calculateTime(IEnumerable<PloggingSession> sessions)
    {
        TimeSpan totalTime = default(TimeSpan);
        foreach(var s in sessions)
        {
            totalTime += (s.EndDate - s.StartDate);
        }
        return totalTime;
    }
}

