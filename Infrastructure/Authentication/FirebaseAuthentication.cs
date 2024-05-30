using System.Diagnostics;
using Firebase.Auth;
using PlogPal.Application.Common.Interfaces;

namespace Infrastructure.Authentication;

public class FirebaseAuthentication : IAuthenticationService
{
    private UserCredential? _userCredential;
    private User CurrentUser => _userCredential?.User;
    private string UserId => CurrentUser?.Uid;
    private string BearerToken => CurrentUser.Credential.IdToken;

    private readonly FirebaseAuthClient _firebaseAuthClient;
    private readonly IUserContext _userContext;

    //TODO can this somehow set usercontext throughout the application
    //Ta bort unused dependencies i olika paket
    public FirebaseAuthentication(FirebaseAuthClient firebaseAuthClient, IUserContext userContext)
    {
        _firebaseAuthClient = firebaseAuthClient;
        _userContext = userContext;
    }

    public async Task<bool> LoginUser(string email, string password)
    {
        try
        {
            _userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task CreateUser(string email, string password, string displayName)
    {
        _userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
    }

    //public async Task<bool> AutoLogin()
    //{
    //    var email = await SecureStorage.GetAsync("email");
    //    var password = await SecureStorage.GetAsync("password");

    //    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
    //    {
    //        return await LoginUser(email, password);
    //    }
    //    else
    //    {
    //        Trace.WriteLine("Autologin failed.");
    //        return false;
    //    }
    //}

    //public async Task SaveCredentials(bool rememberMe, string email, string password)
    //{
    //    if (rememberMe)
    //    {
    //        await SecureStorage.SetAsync("email", email);
    //        await SecureStorage.SetAsync("password", password);
    //    }
    //    else
    //    {
    //        SecureStorage.Remove("email");
    //        SecureStorage.Remove("password");
    //    }
    //}

    public void SignOut()
    {
        _userCredential = null;
        _firebaseAuthClient.SignOut();
    }
}
