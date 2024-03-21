using PloggingApp.Pages;

namespace PloggingApp.MVVM.Views;

public partial class GenerateQRcodeButtonView : ContentView
{
	public GenerateQRcodeButtonView()
	{
		InitializeComponent();
	}

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync($"//{nameof(GenerateQRcodePage)}");
        await Shell.Current.GoToAsync(nameof(GenerateQRcodePage));
    }
}
