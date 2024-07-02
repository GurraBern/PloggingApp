using CommunityToolkit.Maui.Views;

namespace PlogPal.Maui.Features.Dashboard;

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
        Microsoft.Maui.Controls.Application.Current?.MainPage?.ShowPopup(mapIconExplanationsPopup);
    }
}