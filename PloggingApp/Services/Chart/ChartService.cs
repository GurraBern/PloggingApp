﻿using Microcharts;
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
        {LitterType.Plastics, SKColor.Parse("#8564f1")},
        {LitterType.LargePlastics, SKColor.Parse("#bf3a8b")},
        {LitterType.Cigarette, SKColor.Parse("#b346b1")},
        {LitterType.Can, SKColor.Parse("#a453d4")},
        {LitterType.SmallMetal, SKColor.Parse("#a453d4")},
        {LitterType.Cardboard, SKColor.Parse("#874b01")},
        {LitterType.Snus, SKColor.Parse("#bb3333")},
        {LitterType.Glass, SKColor.Parse("#c1345f") },
        {LitterType.Fabric, SKColor.Parse("#fa66f5") }
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
        if (!sessions.Any() || sessions.All(s => !s.PloggingData.Litters.Any()))
            return generateEmptyLitterChart();
        Dictionary<LitterType, double> keyValuePairs = sessions
            .SelectMany(x => x.PloggingData.Litters)
            .GroupBy(g => g.LitterType)
            .ToDictionary(d => d.Key, v => v.Sum(g => g.LitterCount));

        List<ChartEntry> chartEntries =
            keyValuePairs.Select(lv => new ChartEntry((float?)lv.Value) { Label = lv.Key.ToString(), ValueLabel = lv.Value.ToString(), 
                Color = (colors.ContainsKey(lv.Key)) ? colors[lv.Key] : SKColor.Parse("#000000")}).ToList();

        var graph = new BarChart()
        {
            IsAnimated = true,
            //LabelMode = LabelMode.RightOnly,
            //GraphPosition = GraphPosition.AutoFill,

            Entries = chartEntries,
            LabelTextSize = 25f
        };
        return graph;
    }
    private Chart generateEmptyLitterChart()
    {
        var graph = new BarChart()
        {
            IsAnimated = true,
            Entries = new List<ChartEntry>()
            {
                new ChartEntry(1f)
                {
                    Label = "N/A",
                    Color = SKColor.Parse("#dedede")
                }
            },
            LabelTextSize = 20
        };
        return graph;
    }
    public Chart generateLineChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions, Func<PloggingSession, double> func, SKColor color, int year, int month = 1 )
    {
        if (!sessions.Any() || sessions.Sum(func) == 0)
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
                        group => group.Sum(func));
            return makeLineChart(distancePerTimePeriod, timeResolution, color, year, month);
        }
        else
        {
            distancePerTimePeriod = sessions
                    .GroupBy(s => s.StartDate.Month)
                    .ToDictionary(
                        group => new DateTime(year, group.Key, 1),
                        group => group.Sum(func));
            return makeLineChart(distancePerTimePeriod, timeResolution, color, year, month);
        }
    }
    private Chart makeLineChart(Dictionary<DateTime, double> dict, TimeResolution timeRes, SKColor color, int year, int month = 1)
    {
        Dictionary<string, double> valuePerTimePeriod = new Dictionary<string, double>();
        if(timeRes is TimeResolution.ThisMonth)
        {
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                DateTime currentDate = new DateTime(year, month, day);
                double acc = dict.ContainsKey(currentDate) ? dict[currentDate] : (double)0;
                valuePerTimePeriod.Add(currentDate.ToString("d "), acc);
            }
        }
        else
        {
            for (int m = 1; m <= 12; m++)
            {
                DateTime thisMonth = new DateTime(year, m, 1);
                double acc = dict.ContainsKey(thisMonth) ? dict[thisMonth] : 0;
                valuePerTimePeriod.Add(thisMonth.ToString("MMM"), acc);
            }
        }
        //For some reason, if every value in the graph == 0. Skiasharp throws an exception, 
        // claiming that "shaderA is null". This is an ugly workaround for this.
        float offset = (valuePerTimePeriod.Values.All(v => Math.Round(v, 2) == 0)) ? 0.005f : 0f;
        var lineChart = new LineChart
        {
            Entries = valuePerTimePeriod.Select(kv => new ChartEntry((float?)Math.Round((kv.Value), 2) + offset)
            { Label = kv.Key.ToString(), ValueLabel = (kv.Value != 0) ? Math.Round((kv.Value), 2).ToString() : "", Color = color }).ToList(),
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

    public Chart generateEmptyLineChart(TimeResolution tr, int year, int month)
    {
        List<ChartEntry> entries = new List<ChartEntry>();
        if (tr is TimeResolution.ThisYear)
        {
            for (int m = 1; m <= 12; m++)
            {
                var dateTime = new DateTime(year, m, 1);
                // Exception is thrown when 0f is passed into the ChartEntry (???)
                var newEntry = new ChartEntry((float?)1)
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
                var newEntry = new ChartEntry(1f)
                {
                    Label = new DateTime(year, month, day).ToString("d "),
                    ValueLabel = "",
                    Color = SKColor.Parse("#dedede")
                };
                entries.Add(newEntry);
            }
        }
        var chart = new LineChart()
        {
            LineMode = LineMode.Straight,
            PointMode = PointMode.None,
            LabelOrientation = Orientation.Vertical,
            ValueLabelOrientation = Orientation.Horizontal,
            EnableYFadeOutGradient = true,
            IsAnimated = false,
            LabelTextSize = 20f,
            Entries = entries
        };
        return chart;
    }
}
    
