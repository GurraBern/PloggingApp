using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public class DashboardViewModel
{
    public MapViewModel MapViewModel { get; }
    public AddLitterViewModel AddLitterViewModel { get; }
    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StreakViewModel StreakViewModel { get; set; }
    public PlogTogetherViewModel PlogTogetherViewModel {get; set;}

	public DashboardViewModel(MapViewModel mapViewModel,
        AddLitterViewModel addLitterViewModel,
        PloggingSessionViewModel ploggingSessionViewModel,
        StreakViewModel streakViewModel,
        PlogTogetherViewModel plogTogetherViewModel)
	{
        MapViewModel = mapViewModel;
        AddLitterViewModel = addLitterViewModel;
        PloggingSessionViewModel = ploggingSessionViewModel;
        StreakViewModel = streakViewModel;
        PlogTogetherViewModel = plogTogetherViewModel;
	}
}


