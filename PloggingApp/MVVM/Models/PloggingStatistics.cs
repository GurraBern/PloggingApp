using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models;
public partial class PloggingStatistics : ObservableObject
{
    private Total steps { get; set; }
    private Total distance { get; set; }
    private Total cO2Saved { get; set; }
    private Total weight { get; set; }

    private TimeResolution currentTR {get; set;}
    [ObservableProperty]
    public double totalSteps;
    [ObservableProperty]
    public double totalDistance;
    [ObservableProperty]
    public double totalCO2Saved;
    [ObservableProperty]
    public double totalWeight;

    public PloggingStatistics(IEnumerable<PloggingSession> sessions)
    {
        steps = new Total(sessions, s => s.PloggingData.Steps);
        distance = new Total(sessions, s => s.PloggingData.Distance);
        // Add functionality to display CO2 saved
        cO2Saved = new Total();
        weight = new Total(sessions, s => s.PloggingData.Litters.Sum(l => l.Weight));
        TotalSteps = steps.year;
        TotalDistance = distance.year;
        TotalCO2Saved = cO2Saved.year;
        TotalWeight = weight.year;

    }
    public void changeTimeResolution(TimeResolution tr)
    {
        if (tr.Equals(currentTR))
            return;
        currentTR = tr;
        TotalSteps = steps.GetValue(tr);
        TotalDistance = distance.GetValue(tr);
        TotalCO2Saved = cO2Saved.GetValue(tr);
        TotalWeight = weight.GetValue(tr);
    }


}

