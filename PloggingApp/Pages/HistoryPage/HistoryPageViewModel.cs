using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;
public class HistoryPageViewModel
{
    public HistoryViewModel HistoryViewModel { get; set; }
    public StatisticsViewModel StatisticsViewModel { get; set; }    
    public HistoryPageViewModel(HistoryViewModel historyViewModel, StatisticsViewModel statisticsViewModel)
    {
        HistoryViewModel = historyViewModel;
        StatisticsViewModel = statisticsViewModel;
    }
}
