using PloggingApp.Pages.Dashboard;

namespace PloggingApp.Pages;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(removeViewmodel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}