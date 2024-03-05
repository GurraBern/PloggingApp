using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class LoginPage : ContentPage
{  
    public LoginPage(AuthenticationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
