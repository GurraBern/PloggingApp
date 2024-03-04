using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Services.Statistics;
public interface IStatisticsService
{
    public Chart generateLitterGraph(TimeResolution timeResolution);
}
