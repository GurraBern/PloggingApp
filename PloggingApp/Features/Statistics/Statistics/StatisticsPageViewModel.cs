using PloggingApp.Features.Statistics;

namespace PloggingApp.Features.Statistics;

public class StatisticsPageViewModel(StatisticsViewModel statisticsViewModel)
{
    public StatisticsViewModel StatisticsViewModel { get; set; } = statisticsViewModel;
}
