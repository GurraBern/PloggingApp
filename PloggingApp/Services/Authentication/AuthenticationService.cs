using Firebase.Auth;
using System.Diagnostics;
using PloggingApp.Pages;
using Microsoft.Maui.ApplicationModel.Communication;
using CommunityToolkit.Mvvm.ComponentModel;
namespace PloggingApp.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private UserCredential _userCredential;
    public User CurrentUser =>_userCredential?.User;
    
    private readonly FirebaseAuthClient _firebaseAuthClient;

    public AuthenticationService(FirebaseAuthClient firebaseAuthClient)
    {
        _firebaseAuthClient = firebaseAuthClient;
    }

    public async Task LoginUser(string email, string password)
    {
        _userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
        await Application.Current.MainPage.DisplayAlert("Success", "You are being logged in.", "OK");
        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    public async Task CreateUser(string email, string password, string displayName)
    {
        _userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
    }

    public async Task AutoLogin()
    {
        var email = await SecureStorage.GetAsync("email");
        var password = await SecureStorage.GetAsync("password");

        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            _userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        } else
        {
            Trace.WriteLine("Autologin failed.");
        }
    }

    public async Task SaveCredentials(bool rememberMe, string email, string password)
    {
        if (rememberMe)
        {
            await SecureStorage.SetAsync("email", email);
            await SecureStorage.SetAsync("password", password);
        }
        else
        {
            SecureStorage.Remove("email");
            SecureStorage.Remove("password");
        }
    }

    public void SignOut()
    {
        _userCredential = null;
        _firebaseAuthClient.SignOut();
    }
}
