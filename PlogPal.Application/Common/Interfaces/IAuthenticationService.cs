namespace PlogPal.Application.Common.Interfaces;

public interface IAuthenticationService
{
    //IUserContext CurrentUser { get; }
    //string UserId { get; }
    //string BearerToken { get; }
    Task<bool> LoginUser(string email, string password);
    Task CreateUser(string email, string password, string displayName);
    //Task<bool> AutoLogin();
    //Task SaveCredentials(bool rememberMe, string email, string password);
    void SignOut();
}