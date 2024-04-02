using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using SkiaSharp;
namespace PloggingApp.Services.Statistics;
public class ChartService : IChartService
{
    // TODO reorganize, simplify
    private int currentMonth;
    private int currentYear;
    private DateTime currentDate;
    private Dictionary<LitterType, SKColor> colors = new Dictionary<LitterType, SKColor>
    {
        {LitterType.Plastics, SKColor.Parse("#2bb5ff")},
        {LitterType.Cigarette, SKColor.Parse("#ffb53c")},
        {LitterType.Can, SKColor.Parse("#26ffb0")},
        {LitterType.SmallMetal, SKColor.Parse("#be26ff")},
        {LitterType.Cardboard, SKColor.Parse("#874b01")},
    };
    public ChartService()
    {
        currentMonth = DateTime.UtcNow.Month;
        currentYear = DateTime.UtcNow.Year;
        currentDate = DateTime.UtcNow.Date;
    }

    // Split into subfunctions

    public Chart generateLitterChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions)
    {
        if (!sessions.Any())
            return generateEmptyLitterChart();
        Dictionary<LitterType, double> keyValuePairs = sessions
            .SelectMany(x => x.PloggingData.Litters)
            .GroupBy(g => g.LitterType)
            .ToDictionary(d => d.Key, v => v.Sum(g => g.LitterCount));

        List<ChartEntry> chartEntries =
            keyValuePairs.Select(lv => new ChartEntry((float?)lv.Value) { Label = lv.Key.ToString(), ValueLabel = lv.Value.ToString(), Color = colors[lv.Key] }).ToList();

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
    private Chart generateEmptyLitterChart()
    {
        var graph = new DonutChart()
        {
            IsAnimated = true,
            LabelMode = LabelMode.RightOnly,
            GraphPosition = GraphPosition.AutoFill,
            Entries = new List<ChartEntry>()
            {
                new ChartEntry(1f)
                {
                    Label = "N/A",
                    Color = SKColor.Parse("#999999")
                }
            },
            LabelTextSize = 20
        };
        return graph;
    }
    public Chart generateDistanceChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions, int year, int month = 1)
    {
        if (!sessions.Any())
        {
            return generateEmptyLineChart(timeResolution, year, month);
        }
        Dictionary<DateTime, double> distancePerTimePeriod;
        if (timeResolution.Equals(TimeResolution.ThisMonth))
        {
            distancePerTimePeriod = sessions
                    .GroupBy(s => s.StartDate.Date)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Sum(s => s.PloggingData.Distance));
            return generateMonthLineChart(distancePerTimePeriod, "m", year, month);
        }
        else
        {
            distancePerTimePeriod = sessions
                    .GroupBy(s => s.StartDate.Month)
                    .ToDictionary(
                        group => new DateTime(currentYear, group.Key, 1),
                        group => group.Sum(s => s.PloggingData.Distance));
            return generateYearLineChart(distancePerTimePeriod, "m", year);
        }
    }
    private Chart generateMonthLineChart(Dictionary<DateTime, double> dict, string yAxisLabel, int year, int month = 1)
    {

        Dictionary<string, double> valuePerDay = new Dictionary<string, double>();
        for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
        {
            DateTime currentDate = new DateTime(currentYear, currentMonth, day);
            double acc = dict.ContainsKey(currentDate) ? dict[currentDate] : 0;
            valuePerDay.Add(currentDate.ToString("d "), acc);
        }
        return generateLineChart(valuePerDay, yAxisLabel, SKColor.Parse("#9558a8"));
    }

    private Chart generateYearLineChart(Dictionary<DateTime, double> dict, string yAxisLabel, int year)
    {
        Dictionary<string, double> valuePerMonth = new Dictionary<string, double>();
        for (int m = 1; m <= 12; m++)
        {
            DateTime thisMonth = new DateTime(year, m, 1);
            double acc = dict.ContainsKey(thisMonth) ? dict[thisMonth] : 0;
            valuePerMonth.Add(thisMonth.ToString("MMM"), acc);
        }
        return generateLineChart(valuePerMonth, yAxisLabel, SKColor.Parse("#6100b0"));
    }

    // General function
    private Chart generateLineChart(Dictionary<string, double> dict, string yAxisLabel, SKColor color)
    {
        var lineChart = new LineChart
        {
            Entries = dict.Select(kv => new ChartEntry((float?)kv.Value)
            { Label = kv.Key.ToString(), ValueLabel = (kv.Value != 0) ? kv.Value.ToString() : " ", Color = color }).ToList(),
            LineMode = LineMode.Straight,
            PointMode = PointMode.None,
            LabelOrientation = Orientation.Vertical,
            ValueLabelOrientation = Orientation.Vertical,
            EnableYFadeOutGradient = true,
            IsAnimated = true,
            LabelTextSize = 20f
        };
        return lineChart;
    }

    private Chart generateEmptyLineChart(TimeResolution tr, int year, int month)
    {
        List<ChartEntry> entries = new List<ChartEntry>();
        if (tr is TimeResolution.ThisYear)
        {
            for (int m = 1; m <= 12; m++)
            {
                var dateTime = new DateTime(year, m, 1);
                var newEntry = new ChartEntry(0f)
                {
                    Label = dateTime.ToString("MMM"),
                    ValueLabel = "", 
                    Color = SKColor.Parse("#999999")
                };
                entries.Add(newEntry);
            }
        }
        else
        {
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                var newEntry = new ChartEntry(0f)
                {
                    Label = new DateTime(year, month, day).ToString("d "),
                    ValueLabel = "",
                    Color = SKColor.Parse("#999999")
                };
                entries.Add(newEntry);
            }
        }
        var chart = new LineChart()
        {
            LineMode = LineMode.Straight,
            PointMode = PointMode.None,
            LabelOrientation = Orientation.Vertical,
            ValueLabelOrientation = Orientation.Vertical,
            EnableYFadeOutGradient = true,
            IsAnimated = true,
            LabelTextSize = 20f,
            Entries = entries
        };
        return chart;
    }
}
    
