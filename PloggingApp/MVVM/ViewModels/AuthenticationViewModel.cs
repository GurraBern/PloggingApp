using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Authentication;
using System.Diagnostics;
using PloggingApp.Pages;
using PloggingApp.Data.Services;

namespace PloggingApp.MVVM.ViewModels;

//TODO: Logout button
//More information on Firebase package used: https://www.nuget.org/packages/FirebaseAuthentication.net 
public partial class AuthenticationViewModel : ObservableObject, IAsyncInitialization
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IStreakService _streakService;

    [ObservableProperty]
    private bool rememberMeEnabled;
    public string RegEmail { get; set; }
    public string RegPassword { get; set; }
    public string LoginEmail { get; set; }
    public string LoginPassword { get; set; }
    public string DisplayName { get; set; }
    public Task Initialization { get; }

    public AuthenticationViewModel(IAuthenticationService authenticationService, IStreakService streakService)
    {
        _authenticationService = authenticationService;
        _streakService = streakService;

        Initialization = Initialize();
    }


    private async Task Initialize()
    {
        await _authenticationService.AutoLogin();
    }


    [RelayCommand]
    private async Task Logout()
    {
        _authenticationService.SignOut();
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }


    [RelayCommand]
    private async Task GoToLoginPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
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
            }
            catch (Exception ex)
            {
                //Trace.WriteLine($"ERROR: {ex.Message}");

                string errorMessage = ex.Message;

                if (errorMessage.Contains("INVALID_LOGIN_CREDENTIALS"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid login credentials", "OK");
                }
                else if (errorMessage.Contains("INVALID_EMAIL"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid email.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "An error occurred.", "OK");
                }
            }
        }
    }


    [RelayCommand]
    private async Task GoToRegisterPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
    }


    [RelayCommand]
    private async Task Register()
    {
        if (!string.IsNullOrEmpty(RegEmail) && !string.IsNullOrEmpty(RegPassword) && !string.IsNullOrEmpty(DisplayName))
        {
            try
            {
                await _authenticationService.CreateUser(RegEmail, RegPassword, DisplayName);

                string userId = _authenticationService.CurrentUser.Uid;
                await _streakService.CreateUser(userId);

                await Application.Current.MainPage.DisplayAlert("Success", "Account created.", "OK");
                await _authenticationService.LoginUser(RegEmail, RegPassword);
            }
            catch (Exception ex)
            {
                //Trace.WriteLine($"ERROR: {ex.Message}");

                string errorMessage = ex.Message;

                if (errorMessage.Contains("INVALID_EMAIL"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Email invalid.", "OK");
                }
                else if (errorMessage.Contains("WEAK_PASSWORD"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Password must have a minimum length of 6 characters.", "OK");
                }
                else if (errorMessage.Contains("EMAIL_EXISTS"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Email already exists.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "An error occurred.", "OK");
                }
            }
        }
    }

}