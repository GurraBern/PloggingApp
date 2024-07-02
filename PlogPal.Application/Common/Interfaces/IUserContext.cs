namespace PlogPal.Application.Common.Interfaces;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    string UserId { get; }
    string BearerToken { get; }
    public User User { get; }
    Task Login(string email, string password);
}

public class User
{
    public int Streak { get; set; }

    //TODO other User related information
}
