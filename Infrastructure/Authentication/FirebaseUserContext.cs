using PlogPal.Application.Common.Interfaces;

namespace Infrastructure.Authentication;

public class FirebaseUserContext : IUserContext
{
    public bool IsAuthenticated => throw new NotImplementedException();

    public string UserId => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

    public string BearerToken => throw new NotImplementedException();
}
