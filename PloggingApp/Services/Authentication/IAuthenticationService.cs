using Firebase.Auth;
namespace PloggingApp.Services.Authentication;

public interface IAuthenticationService
{
    User CurrentUser { get; }
    Task LoginUser(string email, string password);
    Task CreateUser(string email, string password);
    Task AutoLogin();
    Task SaveCredentials(bool rememberMe, string email, string password);
    void SignOut();
}