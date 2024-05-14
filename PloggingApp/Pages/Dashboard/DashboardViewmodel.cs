using PloggingApp.Features.Map;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public class DashboardViewModel
{
    public MapViewModel MapViewModel { get; }
    public AddLitterViewModel AddLitterViewModel { get; }
    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StreakViewModel StreakViewModel { get; }
    public MyProfileViewModel MyProfileViewModel { get; }

    public DashboardViewModel(MapViewModel mapViewModel,
        AddLitterViewModel addLitterViewModel,
        PloggingSessionViewModel ploggingSessionViewModel,
        StreakViewModel streakViewModel,
        MyProfileViewModel myProfileViewModel)
	{
        MapViewModel = mapViewModel;
        AddLitterViewModel = addLitterViewModel;
        PloggingSessionViewModel = ploggingSessionViewModel;
        StreakViewModel = streakViewModel;
        MyProfileViewModel = myProfileViewModel;
    }
}

