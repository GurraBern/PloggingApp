using Microcharts;
using PlogPal.Domain.Enums;
using PlogPal.Domain.Models;
using SkiaSharp;

namespace PlogPal.Maui.Services.Statistics;

public interface IChartService
{
    public Chart GenerateLitterChart(TimeResolution timeResolution, IEnumerable<PlogSession> sessions);

    public Chart GenerateLineChart(TimeResolution timeResolution, IEnumerable<PlogSession> sessions, Func<PlogSession, double> Func, SKColor color, int year, int month);

    public Chart GenerateEmptyLineChart(TimeResolution tr, int year, int month);

}
