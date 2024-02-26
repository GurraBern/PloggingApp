using Firebase.Auth;
using PloggingApp.Pages;
using System.Diagnostics;
namespace PloggingApp.MVVM.ViewModels;

public class AuthenticationViewModel
{
    private readonly FirebaseAuthClient _firebaseAuthClient;
    //private INavigation _navigation;

    public bool isSwitchToggled { get; set; }

    public Command LoginBtn { get; }
    public Command RegisterBtn { get; }
    public Command RegisterUser { get; }


    public string RegEmail { get; set; }
    public string RegPassword { get; set; }

    public string LoginEmail { get; set; }
    public string LoginPassword { get; set; }


    public AuthenticationViewModel(FirebaseAuthClient firebaseAuthClient)
    {
        this._firebaseAuthClient = firebaseAuthClient;

        Routing.RegisterRoute("dbpage", typeof(DashboardPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("register", typeof(RegisterPage));

        if (_firebaseAuthClient == null)
        {
            Trace.WriteLine("____________FIREBASECLIENT IS NULL___________");
        }

        LoginBtn = new Command(LoginBtnClickedAsync);
        RegisterBtn = new Command(RegisterBtnClickedAsync);
        RegisterUser = new Command(RegisterUserClickedAsync);
    }


    private async void LoginBtnClickedAsync(object obj)
    {
        var loginEmail = LoginEmail;
        var loginPassword = LoginPassword;
        bool switchStatus = isSwitchToggled;

        if (switchStatus) {
            await SecureStorage.SetAsync("loginEmail", loginEmail);
            await SecureStorage.SetAsync("loginPassword", loginPassword);
        }
        else { 
            SecureStorage.RemoveAll();
        }

        try
        {
            var userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(LoginEmail, LoginPassword);

            await Application.Current.MainPage.DisplayAlert("Success", "You are being logged in.", "OK");
            await Shell.Current.GoToAsync("dbpage");

        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
        }

    }


    private async void RegisterBtnClickedAsync(object obj)
    {
        await Shell.Current.GoToAsync("register");

    }


    public async void RegisterUserClickedAsync(object obj)
    {
        var regEmail = RegEmail;
        var regPassword = RegPassword;
        if (!string.IsNullOrEmpty(regEmail) && !string.IsNullOrEmpty(regPassword)) {
            try
            {
                var userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(regEmail, regPassword);
                await Application.Current.MainPage.DisplayAlert("Success", "ACCOUNT CREATED.", "OK");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"ERROR: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }


    public async Task AutoLoginAsync()
    {
        var loginEmail = await SecureStorage.GetAsync("loginEmail");
        var loginPassword = await SecureStorage.GetAsync("loginPassword");

        if (!string.IsNullOrEmpty(loginEmail) && !string.IsNullOrEmpty(loginPassword))
        {
            var userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(loginEmail, loginPassword);
            await Shell.Current.GoToAsync("dbpage");
        }
        else
        {
            Trace.WriteLine("Autologin failed.");
        }
    }
}