using PlogPal.Application.Common.Interfaces;

namespace PlogPal.Application;

public class UserContext : IUserContext
{
    private readonly IAuthenticationService _authenticationService;
    private UserInformation UserInformation { get; set; }
    public string BearerToken => UserInformation.BearerToken;

    public string UserId => UserInformation.UserId;

    public bool IsAuthenticated { get; private set; }

    public UserContext(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task Login(string email, string password)
    {
        UserInformation = await _authenticationService.LoginUser(email, password);

        if(UserInformation != null)
        {
            IsAuthenticated = true;
        }
    }
}
