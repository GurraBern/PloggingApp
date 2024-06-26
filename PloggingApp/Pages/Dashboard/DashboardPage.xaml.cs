using CommunityToolkit.Maui.Views;
using PloggingApp.MVVM.Views;

namespace PloggingApp.Pages;

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