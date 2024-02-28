using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Pages;
using PloggingApp.Services.Camera;

namespace PloggingApp.MVVM.ViewModels;

public partial class AcceptPopupViewModel : ObservableObject
{
    private readonly IPopupService _popupService;
    private readonly ICameraService _cameraService;

    public AcceptPopupViewModel(IPopupService popupService, ICameraService cameraService)
    {
        _popupService = popupService;
        _cameraService = cameraService;
    }

    [RelayCommand]
    private async Task ShowCameraView()
    {
        var imagePath = await _cameraService.TakePhoto();
        await Shell.Current.GoToAsync($"{nameof(CheckoutImagePage)}?ImagePath={imagePath}");
    }
}