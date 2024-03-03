using PloggingApp.MVVM.ViewModels;
using System.Diagnostics;

namespace PloggingApp.Pages;

public partial class LoginPage : ContentPage

{    public LoginPage(AuthenticationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.AutoLoginAsync();
    }

    void OnToggled(object sender, ToggledEventArgs e)
    {
        Microsoft.Maui.Controls.Switch switchControl = (Microsoft.Maui.Controls.Switch)sender;
        bool isSwitchToggled = e.Value;
        ((AuthenticationViewModel)BindingContext).isSwitchToggled = isSwitchToggled;

    }
}
