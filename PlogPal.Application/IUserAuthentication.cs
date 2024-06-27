namespace PlogPal.Application;

public interface IUserAuthentication
{
    Task<bool> LoginUser(string email, string password);

    Task CreateUser(string email, string password, string displayName);
}
