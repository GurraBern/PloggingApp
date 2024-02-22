using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages.Dashboard;

public class DashBoardViewModel
{
	public StreakViewModel StreakViewModel { get; set; }

	public DashBoardViewModel(StreakViewModel streakViewModel)
	{
		StreakViewModel = streakViewModel;
	}
}


