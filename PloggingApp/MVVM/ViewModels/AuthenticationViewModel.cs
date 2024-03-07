using Firebase.Auth;
using PloggingApp.Services.Authentication;
using System.Diagnostics;
namespace PloggingApp.MVVM.ViewModels;

//TODO Fix error messages, logout button 
//More information on Firebase package used: https://www.nuget.org/packages/FirebaseAuthentication.net 
public class AuthenticationViewModel
{
    public bool isSwitchToggled { get; set; }
    public Command LoginBtn { get; }
    public Command RegisterBtn { get; }
    public Command RegisterUser { get; }
    public Command BackBtn { get; }
    public Command LogoutBtn { get; }

    public string RegEmail { get; set; }
    public string RegPassword { get; set; }
    public string LoginEmail { get; set; }
    public string LoginPassword { get; set; }


    private readonly IAuthenticationService _authenticationService;

    public AuthenticationViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;

        BackBtn = new Command(BackButtonClickedAsync);
        LoginBtn = new Command(LoginBtnClickedAsync);
        RegisterBtn = new Command(RegisterBtnClickedAsync);
        RegisterUser = new Command(RegisterUserClickedAsync);
    }

    private async void BackButtonClickedAsync(object obj)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void LoginBtnClickedAsync(object obj)
    {
        var loginEmail = LoginEmail;
        var loginPassword = LoginPassword;
        bool switchStatus = isSwitchToggled;

        if (!string.IsNullOrEmpty(loginEmail) && !string.IsNullOrEmpty(loginPassword))
        {
            if (switchStatus)
            {
                await SecureStorage.SetAsync("loginEmail", loginEmail);
                await SecureStorage.SetAsync("loginPassword", loginPassword);
            }
            else
            {
                SecureStorage.RemoveAll();
            }

            try
            {
                await _authenticationService.SignInWithEmailAndPasswordAsync(loginEmail, loginPassword);

                await Application.Current.MainPage.DisplayAlert("Success", "You are being logged in.", "OK");
                await Shell.Current.GoToAsync("//DashboardPage");

            }
            catch (Exception ex)
            {
                //await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
                await Application.Current.MainPage.DisplayAlert("Error", $"Invalid login credentials", "OK");
            }
        }
    }


    private async void RegisterBtnClickedAsync(object obj)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }


    public async void RegisterUserClickedAsync(object obj)
    {
        var regEmail = RegEmail;
        var regPassword = RegPassword;

        if (!string.IsNullOrEmpty(regEmail) && !string.IsNullOrEmpty(regPassword)) {
            try
            {
                await _authenticationService.CreateUserWithEmailAndPasswordAsync(regEmail, regPassword);
                await Application.Current.MainPage.DisplayAlert("Success", "Account created.", "OK");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"ERROR: {ex.Message}");
                //await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
                await Application.Current.MainPage.DisplayAlert("Error", $"Email exists or password too short.", "OK");
            }
        }
    }


    public async Task AutoLoginAsync()
    {
        var loginEmail = await SecureStorage.GetAsync("loginEmail");
        var loginPassword = await SecureStorage.GetAsync("loginPassword");

        if (!string.IsNullOrEmpty(loginEmail) && !string.IsNullOrEmpty(loginPassword))
        {
            await _authenticationService.SignInWithEmailAndPasswordAsync(loginEmail, loginPassword);
            await Shell.Current.GoToAsync("//DashboardPage");
        }
        else
        {
            Trace.WriteLine("Autologin failed.");
        }
    }
}