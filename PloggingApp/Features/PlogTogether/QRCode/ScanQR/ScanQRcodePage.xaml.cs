namespace PlogPal.Maui.Features.Plogtogether;

public partial class ScanQRcodePage : ContentPage
{
    //private readonly ScanQRcodePageViewModel vm;
    private bool isScanning = true;

    public ScanQRcodePage(/*ScanQRcodePageViewModel vm*/)
    {
        InitializeComponent();

        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = true
        };
        //BindingContext = vm;
        //this.vm = vm;
    }

    private async void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (!isScanning)
        {
            return; // If scanning is disabled, exit the method
        }

        var first = e.Results?.FirstOrDefault();

        barcodeReader.BarcodesDetected -= barcodeReader_BarcodesDetected;

        if (first is null)
        {
            return;
        }

        string userId = first.Value;
        //await vm.AddUserToGroup(userId);

        isScanning = true;
    }

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
    }
}
