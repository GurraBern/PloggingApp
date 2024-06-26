﻿using Firebase.Auth;
using System.Diagnostics;
using PloggingApp.Pages;

namespace PloggingApp.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private UserCredential _userCredential;
    public User CurrentUser =>_userCredential?.User;
    
    private readonly FirebaseAuthClient _firebaseAuthClient;
    private readonly IToastService _toastService;

    public AuthenticationService(FirebaseAuthClient firebaseAuthClient, IToastService toastService)
    {
        _firebaseAuthClient = firebaseAuthClient;
        _toastService = toastService;
    }

    public async Task LoginUser(string email, string password)
    {
        _userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);

        var currentUserId = CurrentUser.Uid;

        await _toastService.MakeToast("You are being logged in.");
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

            var currentUserId = CurrentUser.Uid;

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
