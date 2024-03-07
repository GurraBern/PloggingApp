using Firebase.Auth;
namespace PloggingApp.Services.Authentication;

public interface IAuthenticationService
{
    User CurrentUser { get; }
    Task SignInWithEmailAndPasswordAsync(string email, string password);
    Task CreateUserWithEmailAndPasswordAsync(string email, string password);
    void SignOut();
}