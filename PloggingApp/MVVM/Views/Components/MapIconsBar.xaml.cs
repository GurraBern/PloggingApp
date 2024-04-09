using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace PloggingApp.MVVM.Views;

public partial class MapIconsBar : ContentView
{
    public MapIconsBar()
	{
		InitializeComponent();
	}

    private void ShowIconExplanationsPopup(object sender, TappedEventArgs e)
    {
        var mapIconExplanationsPopup = new MapIconExplanationsPopup();
        Application.Current?.MainPage?.ShowPopup(mapIconExplanationsPopup);
    }
}