using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Services.Statistics;
public interface IStatisticsService
{
    public Chart generateLitterChart(TimeResolution timeResolution);

    public Chart generateDistanceChart(TimeResolution timeResolution); 

    public Chart generateStepsChart(TimeResolution timeResolution);

}
