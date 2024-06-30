using PlogPal.Application.Errors;

namespace PlogPal.Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<UserInformation> LoginUser(string email, string password);
    Task<string> CreateUser(string email, string password, string displayName);
    void SignOut();
}