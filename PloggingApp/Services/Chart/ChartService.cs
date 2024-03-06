using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using SkiaSharp;
using System.Collections.ObjectModel;
namespace PloggingApp.Services.Statistics;
public class ChartService : IChartService
{
    // TODO reorganize, simplify
    private int currentMonth;
    private int currentYear;
    private DateTime currentDate;
    private IEnumerable<PloggingSession> sessions;
    private Dictionary<LitterType, SKColor> colors = new Dictionary<LitterType, SKColor>
    {
        {LitterType.Plastics, SKColor.Parse("#2bb5ff")},
        {LitterType.Cigarette, SKColor.Parse("#ffb53c")},
        {LitterType.Can, SKColor.Parse("#26ffb0")},
        {LitterType.SmallMetal, SKColor.Parse("#be26ff")},
        {LitterType.Cardboard, SKColor.Parse("#874b01")},
    };
    public ChartService(IEnumerable<PloggingSession> Sessions)
    {
        sessions = Sessions;
        currentMonth = DateTime.UtcNow.Month;
        currentYear = DateTime.UtcNow.Year;
        currentDate = DateTime.UtcNow.Date;
    }
    
       // Split into subfunctions
    public Chart generateLitterChart(TimeResolution timeResolution)
    {
        Random rnd = new Random();
        var sortedSessions = filterSessions(timeResolution);

        Dictionary<LitterType, double> keyValuePairs = sortedSessions
            .SelectMany(x => x.PloggingData.Litters)
            .GroupBy(g => g.LitterType)
            .ToDictionary(d => d.Key, v => v.Sum(g => g.LitterCount));

        List<ChartEntry> chartEntries =
            keyValuePairs.Select(lv => new ChartEntry((float?)lv.Value) { Label = lv.Key.ToString(), ValueLabel = lv.Value.ToString(), Color = colors[lv.Key]}).ToList();

        var graph = new DonutChart()
        {
            IsAnimated = true,
            LabelMode = LabelMode.RightOnly,
            GraphPosition = GraphPosition.AutoFill,
            Entries = chartEntries,
            LabelTextSize = 20
        };
        return graph;
    }
    public Chart generateDistanceChart(TimeResolution timeResolution)
    {
        Dictionary<DateTime, double> distancePerTimePeriod;
        var filteredSessions = filterSessions(timeResolution);
        if (timeResolution.Equals(TimeResolution.ThisMonth))
        {
            distancePerTimePeriod = filteredSessions
                    .GroupBy(s => s.StartDate.Date)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Sum(s => s.PloggingData.Distance));
            return generateMonthLineChart(distancePerTimePeriod, "m");
        }
        else
        {
            distancePerTimePeriod = filteredSessions
                    .GroupBy(s => s.StartDate.Month)
                    .ToDictionary(
                        group => new DateTime(currentYear, group.Key, 1),
                        group => group.Sum(s => s.PloggingData.Distance));
            return generateYearLineChart(distancePerTimePeriod, "m");
        }
    }
    public Chart generateStepsChart(TimeResolution timeResolution)
    {
        Dictionary<DateTime, double> distancePerTimePeriod;
        var filteredSessions = filterSessions(timeResolution);
        if (timeResolution.Equals(TimeResolution.ThisMonth))
        {
            distancePerTimePeriod = filteredSessions
                    .GroupBy(s => s.StartDate.Date)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Sum(s => Convert.ToDouble(s.PloggingData.Steps)));
            return generateMonthLineChart(distancePerTimePeriod, "steps");
        }
        else
        {
            distancePerTimePeriod = filteredSessions
                    .GroupBy(s => s.StartDate.Month)
                    .ToDictionary(
                        group => new DateTime(currentYear, group.Key, 1),
                        group => group.Sum(s => Convert.ToDouble(s.PloggingData.Steps)));
            return generateYearLineChart(distancePerTimePeriod, "steps");
        }
    }
    private Chart generateMonthLineChart(Dictionary<DateTime,double> dict, string yAxisLabel)
    {

        Dictionary<string, double> valuePerDay = new Dictionary<string, double>();
        for (int day = 1; day <= DateTime.DaysInMonth(currentYear,currentMonth); day++)
        {
            DateTime currentDate = new DateTime(currentYear, currentMonth, day);
            double acc = dict.ContainsKey(currentDate) ? dict[currentDate] : 0;
            valuePerDay.Add(currentDate.ToString("d/M"), acc);
        }
        return generateLineChart(valuePerDay, yAxisLabel);
    }

    private Chart generateYearLineChart(Dictionary<DateTime,double> dict, string yAxisLabel)
    {
        Dictionary<string, double> valuePerMonth = new Dictionary<string, double>();
        for(int month = 1; month <= 12; month++)
        {
            DateTime thisMonth = new DateTime(currentYear, month, 1);
            double acc = dict.ContainsKey(thisMonth) ? dict[thisMonth] : 0;
            valuePerMonth.Add(thisMonth.ToString("MMMM"), acc);
        }
        return generateLineChart(valuePerMonth, yAxisLabel);
    }

    // General function
    private Chart generateLineChart(Dictionary<string, double> dict, string yAxisLabel)
    {
        Random rnd = new Random();
        var lineChart = new LineChart
        {
            Entries = dict.Select(kv => new ChartEntry((float?)kv.Value) { Label = kv.Key.ToString(), ValueLabel = $"{kv.Value.ToString()} {yAxisLabel}", Color = SKColor.Parse("#6100b0") }).ToList(),
            LineMode = LineMode.Straight,
            PointMode = PointMode.Circle,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
        };
        return lineChart;
    }
    
    // Helper functions
    private IEnumerable<PloggingSession> filterSessions(TimeResolution timeResolution)
    {
        DateTime limit;
        switch (timeResolution)
        {
                case TimeResolution.ThisMonth:
                limit = new DateTime(currentYear, currentMonth, 1);
                break;
            default:
                limit = new DateTime(currentYear, 1, 1);
                break;
        }
        return sessions.Where(s => s.StartDate > limit);
    }
 }
