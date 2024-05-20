namespace PloggingApp.Features.Authentication;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(AuthenticationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

}

