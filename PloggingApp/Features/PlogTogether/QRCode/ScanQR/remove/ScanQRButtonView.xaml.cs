using PloggingApp.Pages;
using PloggingApp.Data.Services;

namespace PloggingApp.MVVM.Views;

public partial class ScanQRButtonView : ContentView
{
    public ScanQRButtonView()
    {
        InitializeComponent();
    }

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ScanQRcodePage));
        //await Shell.Current.GoToAsync($"//{nameof(ScanQRcodePage)}");
    }
}
