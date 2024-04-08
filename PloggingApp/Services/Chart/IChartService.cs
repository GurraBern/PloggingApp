using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Services.Statistics;
public interface IChartService
{
    public Chart generateLitterChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions);

    public Chart generateDistanceChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions, int year, int month);

    public Chart generateEmptyLineChart(TimeResolution tr, int year, int month);

}
