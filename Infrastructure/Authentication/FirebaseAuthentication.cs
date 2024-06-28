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

    //TODO double check null values
    public async Task<string> LoginUser(string email, string password)
    {
        try
        {
            _userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);

            return _userCredential.User.Credential.IdToken;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public async Task<bool> CreateUser(string email, string password, string displayName)
    {
        _userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);

        return _userCredential != null;
    }

    public void SignOut()
    {
        _userCredential = null;
        _firebaseAuthClient.SignOut();
    }
}
