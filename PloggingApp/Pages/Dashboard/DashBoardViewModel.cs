using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages.Dashboard;

public class DashBoardViewModel
{
	public StreakViewModel StreakViewModel { get; set; }
	public PlogTogetherViewModel PlogTogetherViewModel { get; set; }
	public GenerateQRcodeViewModel GenerateQRcodeViewModel { get; set; }

	public DashBoardViewModel(StreakViewModel streakViewModel, PlogTogetherViewModel plogTogetherViewModel,
							  GenerateQRcodeViewModel generateQRcodeViewModel)
	{
		StreakViewModel = streakViewModel;
		PlogTogetherViewModel = plogTogetherViewModel;
		GenerateQRcodeViewModel = generateQRcodeViewModel;
	}
}


