using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models;
public partial class PloggingStatistics : ObservableObject
{
    private Total<double> steps;
    private Total<double> distance;
    private Total<double> cO2Saved;
    private Total<double> weight;
    private Total<TimeSpan> time;

    private bool isSingleSession { get; set; } = true;
    private TimeResolution currentTR {get; set;}
    // Remove steps altogether?
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
        steps = new Total<double>(sessions, s => s.PloggingData.Steps);
        distance = new Total<double>(sessions, s => s.PloggingData.Distance);
        // Add functionality to display CO2 saved
        cO2Saved = new Total<double>();
        weight = new Total<double>(sessions, s => s.PloggingData.Litters.Sum(l => l.Weight));
        time = new Total<TimeSpan>(sessions, s => s.EndDate - s.StartDate);
        TotalSteps = Math.Round(steps.year, 2);
        TotalDistance = Math.Round(distance.year, 2);
        TotalCO2Saved = Math.Round(cO2Saved.year, 2);
        TotalWeight = Math.Round(weight.year, 2); 
        TotalTime = time.year;
    }
    public PloggingStatistics(PloggingSession session)
    {
        TotalSteps = session.PloggingData.Steps;
        TotalDistance = session.PloggingData.Distance;
        TotalWeight = session.PloggingData.Litters.Sum(s => s.Weight);
        TotalTime = session.EndDate - session.StartDate;
    }
    public void changeTimeResolution(TimeResolution tr)
    {
        if (tr.Equals(currentTR) || !isSingleSession)
            return;
        currentTR = tr;
        TotalSteps = steps.GetValue(tr);
        TotalDistance = distance.GetValue(tr);
        TotalCO2Saved = cO2Saved.GetValue(tr);
        TotalWeight = weight.GetValue(tr);
    }


}

