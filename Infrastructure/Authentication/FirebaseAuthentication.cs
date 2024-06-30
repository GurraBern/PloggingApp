using Firebase.Auth;
using PlogPal.Application;
using PlogPal.Application.Common.Interfaces;

namespace Infrastructure.Authentication;

public class FirebaseAuthentication : IAuthenticationService
{
    private UserCredential? _userCredential;
    private User CurrentUser => _userCredential?.User;
    private string UserId => CurrentUser?.Uid;

    private readonly FirebaseAuthClient _firebaseAuthClient;

    //TODO can this somehow set usercontext throughout the application
    //Ta bort unused dependencies i olika paket
    public FirebaseAuthentication(FirebaseAuthClient firebaseAuthClient)
    {
        _firebaseAuthClient = firebaseAuthClient;
    }

    public async Task<UserInformation> LoginUser(string email, string password)
    {
        try
        {
            _userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            var userInformation = new UserInformation()
            {
                UserId = UserId,
                BearerToken = _userCredential.User.Credential.IdToken
            };

            return userInformation;
        }
        catch (Exception)
        {
            return null; //TODO fix result pattern
        }
    }

    public async Task<string> CreateUser(string email, string password, string displayName)
    {
        _userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
        return UserId;
    }

    public void SignOut()
    {
        _userCredential = null;
        _firebaseAuthClient.SignOut();
    }
}
