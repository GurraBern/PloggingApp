using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(AuthenticationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}

