using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using SkiaSharp;

namespace PloggingApp.Services.Statistics;
public interface IChartService
{
    public Chart generateLitterChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions);

    public Chart generateLineChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions, Func<PloggingSession, double> Func, SKColor color, int year, int month);

    public Chart generateEmptyLineChart(TimeResolution tr, int year, int month);

}
