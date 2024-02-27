namespace PloggingApp.Pages;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewmodel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}