using PlogPal.Maui.Features.Map;
using PlogPal.Maui.Features.PloggingSession;
using PlogPal.Maui.Features.Streak;
using PlogPal.Maui.Pages.PloggingSession;

namespace PlogPal.Maui.Features.Dashboard;

public class DashboardViewModel(MapViewModel mapViewModel,
    AddLitterViewModel addLitterViewModel,
    PloggingSessionViewModel ploggingSessionViewModel,
    StreakViewModel streakViewModel)
{
    public MapViewModel MapViewModel { get; } = mapViewModel;
    public AddLitterViewModel AddLitterViewModel { get; } = addLitterViewModel;
    public PloggingSessionViewModel PloggingSessionViewModel { get; } = ploggingSessionViewModel;
    public StreakViewModel StreakViewModel { get; } = streakViewModel;
}

