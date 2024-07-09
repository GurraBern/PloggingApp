using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlogPal.Application;
using PlogPal.Application.LoginManagement.Commands;
using PlogPal.Maui.Features.Dashboard;
using PlogPal.Maui.Shared;

namespace PlogPal.Maui.Features.Authentication;

public partial class LoginViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly ILoginCommand _loginCommand;
    private readonly IToastService _toastService;
    public string LoginEmail { get; set; }
    public string LoginPassword { get; set; }

    [ObservableProperty]
    private bool rememberMeEnabled;

    public Task Initialization { get; }

    public LoginViewModel(ILoginCommand loginCommand, IToastService toastService)
    {
        _loginCommand = loginCommand;
        _toastService = toastService;

        Initialization = Initialize();
    }

    private async Task Initialize()
    {
        IsBusy = true;

        // await AutoLogin();

        IsBusy = false;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (!string.IsNullOrEmpty(LoginEmail) && !string.IsNullOrEmpty(LoginPassword))
        {
            var result = await _loginCommand.LoginUser(LoginEmail, LoginPassword);
            if(result.IsSuccess)
            {
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");

                await SaveCredentials(RememberMeEnabled);
            }
            else
            {
                await _toastService.MakeToast("Unable to login");
            }
        }
    }

    public async Task AutoLogin()
    {
        var email = await SecureStorage.GetAsync("email");
        var password = await SecureStorage.GetAsync("password");

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return;

        var result = await _loginCommand.LoginUser(email, password);

        if(result.IsSuccess)
            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        else
            await _toastService.MakeToast(result.Error.Description);
    }

    private async Task SaveCredentials(bool rememberMe)
    {
        if (rememberMe)
        {
            await SecureStorage.SetAsync("email", LoginEmail);
            await SecureStorage.SetAsync("password", LoginPassword);
        }
    }

    [RelayCommand]
    private async Task GoToRegisterPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
    }
}
