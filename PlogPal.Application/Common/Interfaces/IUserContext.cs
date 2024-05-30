namespace PlogPal.Application.Common.Interfaces;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    string UserId { get; }
    string Name { get; }
    string BearerToken { get; }
}
