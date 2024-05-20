using CommunityToolkit.Maui.Views;

namespace PloggingApp.Features.Dashboard;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void ShowTutorialPopup(object sender, EventArgs e)
    {
        var mapIconExplanationsPopup = new TutorialPopup();
        Application.Current?.MainPage?.ShowPopup(mapIconExplanationsPopup);
    }
}