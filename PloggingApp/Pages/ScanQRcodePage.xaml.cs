using PloggingApp.Data.Services;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class ScanQRcodePage : ContentPage
{
    private readonly PlogTogetherViewModel vm;

    public ScanQRcodePage(PlogTogetherViewModel vm)
	{
		InitializeComponent();

		barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
		{
			Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
			AutoRotate = true
		};
		BindingContext = vm;
        this.vm = vm;
    }

    private async void barcodeReader_BarcodesDetected(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();

		if(first is null)
		{
			return;
		}
		
		var userId = first.Value;
		await vm.AddUserToGroup(userId);
    }
}
