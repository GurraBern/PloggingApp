using CommunityToolkit.Maui.Views;

namespace PloggingApp.MVVM.Views;

public partial class InformationExpander : ContentView
{
	public InformationExpander()
	{
		InitializeComponent();
	}

    private void ShowTutorialPopup(object sender, EventArgs e)
    {
        var mapIconExplanationsPopup = new TutorialPopup();
        Application.Current?.MainPage?.ShowPopup(mapIconExplanationsPopup);
    }
}