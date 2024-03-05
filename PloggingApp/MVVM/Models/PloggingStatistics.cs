using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models;
public class PloggingStatistics
{
    public Total Steps;
    public Total Distance;
    public Total CO2Saved;
    public Total Weight;

    public PloggingStatistics(IEnumerable<PloggingSession> sessions)
    {
        Steps = new Total(sessions, s => s.PloggingData.Steps);
        Distance = new Total(sessions, s => s.PloggingData.Distance);
        // Add functionality to display CO2 saved
        CO2Saved = new Total();
        Weight = new Total(sessions, s => s.PloggingData.Litters.Sum(l => l.Weight));
    }
}
