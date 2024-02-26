using PloggingApp.MVVM.ViewModels;
namespace PloggingApp.Pages;
public class StatisticsPageViewModel 
{
    public StatisticsViewModel StatisticsViewModel { get; set; }
    public StatisticsPageViewModel()
    {
        StatisticsViewModel = new StatisticsViewModel();
    }
}
