using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Camera;
using PloggingApp.Services.PloggingTracking;

namespace PloggingApp.Pages;

[QueryProperty(nameof(ImagePath), nameof(ImagePath))]
public partial class CheckoutImageViewModel : ObservableObject
{
    [ObservableProperty]
    private string imagePath;
    private readonly ICameraService _cameraService;
    private readonly IPloggingSessionTracker _ploggingSessionTracker;

    public CheckoutImageViewModel(ICameraService cameraService, IPloggingSessionTracker ploggingSessionTracker)
    {
        _cameraService = cameraService;
        _ploggingSessionTracker = ploggingSessionTracker;
    }

    [RelayCommand]
    private async Task UsePhoto()
    {
        await _ploggingSessionTracker.EndSession();

        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    [RelayCommand]
    private async Task RetakePhoto()
    {
        var imagePath = await _cameraService.TakePhoto();
        ImagePath = imagePath;
    }
}
