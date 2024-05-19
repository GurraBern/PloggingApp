using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class GenerateQRcodePage : ContentPage
{
	public GenerateQRcodePage(GenerateQRcodeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
