using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using PloggingApp.Services.Authentication;
using System.Diagnostics;
using PloggingApp.Pages;
namespace PloggingApp.MVVM.ViewModels;

//TODO Fix error messages, logout button 
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
        await AutoLoginAsync();
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
            if (RememberMeEnabled)
            {
                await SecureStorage.SetAsync("loginEmail", LoginEmail);
                await SecureStorage.SetAsync("loginPassword", LoginPassword);
            }
            else
            {
                SecureStorage.Remove("loginEmail");
                SecureStorage.Remove("loginPassword");
            }

            try
            {
                await _authenticationService.SignInWithEmailAndPasswordAsync(LoginEmail, LoginPassword);

                await Application.Current.MainPage.DisplayAlert("Success", "You are being logged in.", "OK");
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
            catch (Exception ex)
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
    public async Task Register()
    {
        if (!string.IsNullOrEmpty(RegEmail) && !string.IsNullOrEmpty(RegPassword))
        {
            try
            {
                await _authenticationService.CreateUserWithEmailAndPasswordAsync(RegEmail, RegPassword);
                await _authenticationService.SignInWithEmailAndPasswordAsync(RegEmail, RegPassword);
                await Application.Current.MainPage.DisplayAlert("Success", "Account created. You are being logged in.", "OK");
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"ERROR: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Email exists or password too short.", "OK");
            }
        }
    }


    private async Task AutoLoginAsync()
    {
        var loginEmail = await SecureStorage.GetAsync("loginEmail");
        var loginPassword = await SecureStorage.GetAsync("loginPassword");

        if (!string.IsNullOrEmpty(loginEmail) && !string.IsNullOrEmpty(loginPassword))
        {
            await _authenticationService.SignInWithEmailAndPasswordAsync(loginEmail, loginPassword);
            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        }
        else
        {
            Trace.WriteLine("Autologin failed.");
        }
    }
}