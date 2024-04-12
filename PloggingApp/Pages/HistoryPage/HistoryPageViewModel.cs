using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;
public class HistoryPageViewModel
{
    public HistoryViewModel HistoryViewModel { get; set; }
    public HistoryPageViewModel(HistoryViewModel historyViewModel)
    {
        HistoryViewModel = historyViewModel;
    }
}
