namespace PloggingApp.Pages;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashBoardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}