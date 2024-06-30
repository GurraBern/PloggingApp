namespace PlogPal.Application.Common.Interfaces;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    string UserId { get; }
    string BearerToken { get; }
    Task Login(string email, string password);
}
