using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Features.UserProfiles.Badges;
using PloggingApp.Shared;

namespace PloggingApp.Features.UserProfiles;

public partial class MyProfilePageViewModel: BaseViewModel
{
    [ObservableProperty]
    private ProfileInfoViewModel profileInfoViewModel;

    [ObservableProperty]
    private BadgesViewModel badgesViewModel;

    public MyProfilePageViewModel(ProfileInfoViewModel profileInfoViewModel, BadgesViewModel badgesViewModel)
    {
        ProfileInfoViewModel = profileInfoViewModel;
        BadgesViewModel = badgesViewModel;
    }

    [RelayCommand]
    private async Task Refresh()
    {
        IsBusy = true;
        //IsRefreshing = false;
        IsBusy = false;
    }

    [RelayCommand]
    private async Task GoToHistoryPage()
    {
        await Shell.Current.GoToAsync($"{nameof(HistoryPage)}");
    }

   
    //[RelayCommand]
    //private async Task Logout()
    //{
    //    bool response = await Application.Current.MainPage.DisplayAlert("Signing out", "Are you sure you want to logout?", "Yes", "No");

    //    if (response)
    //    {
    //        _authenticationService.SignOut();
    //        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    //    }
    //}

    //[RelayCommand]
    //private async Task Refresh()
    //{
    //    IsBusy = true;
    //    await GetSessions();
    //    IsRefreshing = false;
    //    IsBusy = false;
    //}

    //[RelayCommand]
    //private async Task GoToHistoryPage()
    //{
    //    await Shell.Current.GoToAsync($"{nameof(HistoryPage)}");
    //}
}
