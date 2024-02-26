using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class LoginPage : ContentPage
{
    private readonly AuthenticationViewModel vm;
    public LoginPage(AuthenticationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        vm.AutoLoginAsync();
    }

    void OnToggled(object sender, ToggledEventArgs e)
    {
        Switch switchControl = (Switch)sender;
        bool isSwitchToggled = e.Value;
        vm.isSwitchToggled = isSwitchToggled;
    }
}
