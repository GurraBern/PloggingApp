namespace PloggingApp.Features.Authentication;

public partial class LoginPage : ContentPage
{    
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
