﻿using Microcharts;
using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Services.Statistics;
public interface IChartService
{
    public Chart generateLitterChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions);

    public Chart generateDistanceChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions); 

    public Chart generateStepsChart(TimeResolution timeResolution, IEnumerable<PloggingSession> sessions);

}
