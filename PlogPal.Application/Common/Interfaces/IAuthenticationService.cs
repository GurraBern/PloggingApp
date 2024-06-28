namespace PlogPal.Application.Common.Interfaces;

//TODO result pattern
public interface IAuthenticationService
{
    Task<bool> LoginUser(string email, string password);
    Task<string> CreateUser(string email, string password, string displayName);
    void SignOut();
}