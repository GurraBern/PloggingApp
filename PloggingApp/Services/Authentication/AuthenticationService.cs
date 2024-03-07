using Firebase.Auth;

namespace PloggingApp.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private UserCredential _userCredential;
    public User CurrentUser => _userCredential?.User;
    
    private readonly FirebaseAuthClient _firebaseAuthClient;

    public AuthenticationService(FirebaseAuthClient firebaseAuthClient)
    {
        _firebaseAuthClient = firebaseAuthClient;
    }

    public async Task SignInWithEmailAndPasswordAsync(string email, string password)
    {
        _userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
    }

    public async Task CreateUserWithEmailAndPasswordAsync(string email, string password)
    {
        _userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);
    }
    public void SignOut()
    {
        _userCredential = null;
        _firebaseAuthClient.SignOut();
    }
}
