using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Extensions;
using PloggingApp.Features.Statistics;
using PloggingApp.Features.UserProfiles.Badges;
using PloggingApp.Shared;
using PlogPal.Domain.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.Features.UserProfiles;

public partial class MyProfilePageViewModel : BaseViewModel, IAsyncInitialization
{
    [ObservableProperty]
    private ProfileInfoViewModel profileInfoViewModel;

    [ObservableProperty]
    private BadgesViewModel badgesViewModel;

    public ObservableCollection<PlogSession> LatestSessions { get; set; } = [];

    private readonly IPloggingSessionService _ploggingSessionService;
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty]
    private PloggingStatistics ploggingStatistics;

    public Task Initialization {  get; private set; }

    public MyProfilePageViewModel(ProfileInfoViewModel profileInfoViewModel, BadgesViewModel badgesViewModel, IPloggingSessionService ploggingSessionService, IAuthenticationService authenticationService)
    {
        ProfileInfoViewModel = profileInfoViewModel;
        BadgesViewModel = badgesViewModel;
        _ploggingSessionService = ploggingSessionService;
        _authenticationService = authenticationService;

        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        var plogSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.UserId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);

        SetupStatistics(plogSessions);
        LatestSessions.ClearAndAddRange(plogSessions.Take(3));
    }

    private void SetupStatistics(IEnumerable<PlogSession> plogSessions)
    {
        PloggingStatistics = new PloggingStatistics(plogSessions.Where(s => s.StartDate.Month == DateTime.Now.Month));
    }

    [RelayCommand]
    private async Task GoToHistoryPage()
    {
        await Shell.Current.GoToAsync($"{nameof(HistoryPage)}");
    }
}
