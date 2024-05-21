using PloggingApp.Features.Map;
using PloggingApp.Features.PloggingSession;
using PloggingApp.Features.Streak;
using PloggingApp.Features.UserProfiles;

namespace PloggingApp.Features.Dashboard;

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

