using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class GenerateQRcodePage : ContentPage
{
	public GenerateQRcodePage(GenerateQRcodeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
    }
}
