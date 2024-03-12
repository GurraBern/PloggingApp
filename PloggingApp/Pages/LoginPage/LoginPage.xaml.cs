using PloggingApp.MVVM.ViewModels;
using System.Diagnostics;

namespace PloggingApp.Pages;

public partial class LoginPage : ContentPage

{    public LoginPage(AuthenticationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
