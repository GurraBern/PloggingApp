using PloggingApp.Data.Services;
using PloggingApp.MVVM.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services.Interfaces;
using Plogging.Core.Models;
using PloggingApp.Extensions;
using CommunityToolkit.Maui.ApplicationModel;
using Badge = PloggingApp.MVVM.Models.Badge;
namespace PloggingApp.MVVM.ViewModels;

public partial class BadgesViewModel : BaseViewModel
{
    private readonly IPloggingSessionService _ploggingSessionService;

    public ObservableCollection<PloggingSession> UserSessions { get; set; } = [];
    private IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();

    public Task Initialization { get; private set; }
    public ObservableCollection<Badge> Badges { get; set; } = [];
    public BadgesViewModel(IPloggingSessionService ploggingSessionService)
    {
        _ploggingSessionService = ploggingSessionService;
        Initialization = InitializeAsync();
    }
    private async Task InitializeAsync()
    {
        await GetBadges("hej");
    }

    public async Task GetBadges(string UserId)
    {

        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions("TODOsetUserId", DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        UserSessions.ClearAndAddRange(_allUserSessions);
        var PloggingStats = new PloggingStatistics(UserSessions);
        Badges.Add(new TrashCollectedBadge(PloggingStats));
        Badges.Add(new DistanceBadge(PloggingStats));

    }
}