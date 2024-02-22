namespace PloggingApp.Pages;

public partial class LoginPage : ContentPage
{
    private AuthenticationViewModel viewModel;

    public LoginPage()
    {
        InitializeComponent();
        viewModel = new AuthenticationViewModel(Navigation);
        BindingContext = viewModel;

        viewModel.AutoLoginAsync();
    }

    void OnToggled(object sender, ToggledEventArgs e)
    {
        Switch switchControl = (Switch)sender;
        bool isSwitchToggled = e.Value;
        viewModel.isSwitchToggled = isSwitchToggled;
    }
}
