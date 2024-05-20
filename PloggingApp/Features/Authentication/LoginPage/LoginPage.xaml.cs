namespace PloggingApp.Features.Authentication;

public partial class LoginPage : ContentPage

{    public LoginPage(AuthenticationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
