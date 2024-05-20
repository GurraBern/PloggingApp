namespace PloggingApp.Features.Plogtogether;

public partial class GenerateQRcodePage : ContentPage
{
	public GenerateQRcodePage(GenerateQRcodeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
