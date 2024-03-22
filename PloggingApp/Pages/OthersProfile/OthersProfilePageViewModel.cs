using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;
namespace PloggingApp.Pages;

public partial class OthersProfilePageViewModel
{
    public OthersSessionsViewModel OthersSessionsViewModel { get; set; }
    public OthersProfilePageViewModel(IPloggingSessionService sessionService, IUserInfoService userService)
    {
        OthersSessionsViewModel = new OthersSessionsViewModel(sessionService, userService);


    }
    [RelayCommand]
    public async Task GoBack()
    {
        await Shell.Current.GoToAsync($"//{nameof(RankingPage)}");
    }



}
