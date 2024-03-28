using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;
public class StatisticsPageViewModel
{
    public StatisticsViewModel StatisticsViewModel { get; set; }
    public MyProfileViewModel MyProfileViewModel { get; set; }
    public StatisticsPageViewModel(StatisticsViewModel statisticsViewModel, MyProfileViewModel myProfileViewModel)
    {
        StatisticsViewModel = statisticsViewModel;
        MyProfileViewModel = myProfileViewModel;    
    }
}
