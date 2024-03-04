using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public class DashBoardViewModel
{
    public MapViewModel MapViewModel { get; }
    public AddLitterViewModel AddLitterViewModel { get; }
    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StreakViewModel StreakViewModel { get; set; }

	public DashBoardViewModel(MapViewModel mapViewModel,
        AddLitterViewModel addLitterViewModel,
        PloggingSessionViewModel ploggingSessionViewModel,
        StreakViewModel streakViewModel)
	{
        MapViewModel = mapViewModel;
        AddLitterViewModel = addLitterViewModel;
        PloggingSessionViewModel = ploggingSessionViewModel;
        StreakViewModel = streakViewModel;
	}
}


