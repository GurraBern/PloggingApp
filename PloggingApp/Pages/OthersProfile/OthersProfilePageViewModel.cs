using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class OthersProfilePageViewModel : BaseViewModel
{
    public OthersSessionsViewModel OthersSessionsViewModel { get; set; }
    public BadgesViewModel BadgesViewModel { get; set; }
    public OthersProfilePageViewModel(IPloggingSessionService sessionService, IUserInfoService userService, IStreakService streakService, IPopupService popupService)
    {
        OthersSessionsViewModel = new OthersSessionsViewModel(sessionService, userService, streakService, popupService);
        BadgesViewModel = new BadgesViewModel(sessionService, userService, streakService, popupService);


    }


}
