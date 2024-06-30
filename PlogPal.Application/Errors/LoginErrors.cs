namespace PlogPal.Application.Errors;

public class LoginErrors
{
    public static Error InvalidCredentials = new Error("Login.InvalidCredentials", "Incorrect login details");
    public static Error AccountNotCreated = new Error("Login.AccountNotCreated", "Account could not be created");

}
