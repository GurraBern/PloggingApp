using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlogPal.Application.Common.Interfaces;
using PlogPal.Maui.Features.Dashboard;
using PlogPal.Maui.Shared;

namespace PloggingApp.Features.Authentication;

public partial class LoginViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly IAuthenticationService _authenticationService;
    //private readonly IStreakService _streakService;
    //private readonly IUserInfoService _userInfoService;
    private readonly IToastService _toastService;

    public string LoginEmail { get; set; }
    public string LoginPassword { get; set; }

    [ObservableProperty]
    private bool rememberMeEnabled;

    public Task Initialization { get; }

    public LoginViewModel(IAuthenticationService authenticationService, IToastService toastService)
    {
        _authenticationService = authenticationService;
        _toastService = toastService;

        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        IsBusy = true;

        //var isLoginSuccessful = await _authenticationService.AutoLogin();
       
        IsBusy = false;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (!string.IsNullOrEmpty(LoginEmail) && !string.IsNullOrEmpty(LoginPassword))
        {
           
            var isLoggedIn = await _authenticationService.LoginUser(LoginEmail, LoginPassword);
            if(isLoggedIn)
            {
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
            else
            {
                await _toastService.MakeToast("Unable to login");
            }

            //await _authenticationService.SaveCredentials(RememberMeEnabled, LoginEmail, LoginPassword);
            //await _streakService.ResetStreak();
        }
    }

    [RelayCommand]
    private async Task GoToRegisterPage()
    {
        //await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
    }
}
