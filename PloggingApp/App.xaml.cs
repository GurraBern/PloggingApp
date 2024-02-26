namespace PloggingApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        //TODO remove if we want to allow both Light mode and Dark mode
        UserAppTheme = AppTheme.Light;
    }
}
