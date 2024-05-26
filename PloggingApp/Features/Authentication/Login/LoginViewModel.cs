using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Data.Services;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;

namespace PloggingApp.Features.Authentication;

public partial class LoginViewModel: BaseViewModel, IAsyncInitialization
{
    private readonly IAuthenticationService _authenticationService;
	private readonly IStreakService _streakService;
	private readonly IUserInfoService _userInfoService;
	private readonly IToastService _toastService;

    public string LoginEmail { get; set; }
    public string LoginPassword { get; set; }

    [ObservableProperty]
	private bool rememberMeEnabled;

    public Task Initialization { get; }

    public LoginViewModel(IAuthenticationService authenticationService, IStreakService streakService, IUserInfoService userInfoService, IToastService toastService)
    {
        _authenticationService = authenticationService;
        _streakService = streakService;
        _userInfoService = userInfoService;
        _toastService = toastService;
        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        IsBusy = true;

        var isLoginSuccessful = await _authenticationService.AutoLogin();
        if (isLoginSuccessful)
        {
            await _streakService.ResetStreak();
        }

        IsBusy = false;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (!string.IsNullOrEmpty(LoginEmail) && !string.IsNullOrEmpty(LoginPassword))
        {
            try
            {
                await _authenticationService.LoginUser(LoginEmail, LoginPassword);
                await _authenticationService.SaveCredentials(RememberMeEnabled, LoginEmail, LoginPassword);
                await _streakService.ResetStreak();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid login credentials", "OK");
            }
        }
    }

    [RelayCommand]
    private async Task GoToRegisterPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
    }
}
