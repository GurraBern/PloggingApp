using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using SkiaSharp;
using System.Collections.ObjectModel;
namespace PloggingApp.Services.Statistics;
public class StatisticsService : IStatisticsService
{
    private IEnumerable<PloggingSession> sessions;
    private Dictionary<LitterType, SKColor> colors = new Dictionary<LitterType, SKColor>
    {
        {LitterType.Plastics, SKColor.Parse("#2bb5ff")},
        {LitterType.Cigarette, SKColor.Parse("#ffb53c")},
        {LitterType.Can, SKColor.Parse("#26ffb0")},
        {LitterType.SmallMetal, SKColor.Parse("#be26ff")},
        {LitterType.Cardboard, SKColor.Parse("#874b01")},
    };
    public StatisticsService(IEnumerable<PloggingSession> Sessions)
    {
        sessions = Sessions;
    }
    
       // Split into subfunctions
    public Chart generateLitterGraph(TimeResolution timeResolution)
    {
        Random rnd = new Random();
        DateTime limit = timeSortLimit(timeResolution);

        var sortedSessions = sessions.Select(s => s.StartDate < DateTime.UtcNow.AddDays(-7));

        Dictionary<LitterType, double> keyValuePairs = sessions
            .SelectMany(x => x.PloggingData.Litters)
            .GroupBy(g => g.LitterType)
            .ToDictionary(d => d.Key, v => v.Sum(g => g.LitterCount));

        List<ChartEntry> chartEntries =
            keyValuePairs.Select(lv => new ChartEntry(rnd.Next()) { Label = lv.Key.ToString(), ValueLabel = lv.Value.ToString(), Color = colors[lv.Key]}).ToList();

        var graph = new PieChart()
        {
            IsAnimated = false,
            Entries = chartEntries,
            LabelTextSize = 20
        };
        return graph;
    }

    private DateTime timeSortLimit(TimeResolution timeResolution)
    {
        DateTime limit;
        switch (timeResolution)
        {
            case TimeResolution.ThisWeek:
                limit = DateTime.UtcNow.AddDays(-7);
                break;
            case TimeResolution.ThisMonth:
                limit = DateTime.UtcNow.AddMonths(-1);
                break;
            default:
                limit = DateTime.UtcNow.AddYears(-1);
                break;
        }
        return limit;
    }
}
