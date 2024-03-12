using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Authentication;
using System.Diagnostics;
using PloggingApp.Pages;
namespace PloggingApp.MVVM.ViewModels;

//TODO: Logout button, error messages
//More information on Firebase package used: https://www.nuget.org/packages/FirebaseAuthentication.net 
public partial class AuthenticationViewModel : ObservableObject, IAsyncInitialization
{
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty]
    private bool rememberMeEnabled;
    public string RegEmail { get; set; }
    public string RegPassword { get; set; }
    public string LoginEmail { get; set; }
    public string LoginPassword { get; set; }
    public Task Initialization { get; }

    public AuthenticationViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;

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
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Invalid login credentials", "OK");
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
        if (!string.IsNullOrEmpty(RegEmail) && !string.IsNullOrEmpty(RegPassword))
        {
            try
            {
                await _authenticationService.CreateUser(RegEmail, RegPassword);
                await Application.Current.MainPage.DisplayAlert("Success", "Account created.", "OK");
                await _authenticationService.LoginUser(RegEmail, RegPassword);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"ERROR: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Email exists or password too short.", "OK");
            }
        }
    }

}