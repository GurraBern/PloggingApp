using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PloggingApp.Services.Camera;
using PlogPal.Application.Common.Interfaces;
using PlogPal.Maui.Features.Dashboard;

namespace PloggingApp.Features.PloggingSession;

[QueryProperty(nameof(ImagePath), nameof(ImagePath))]
public partial class CheckoutImageViewModel : ObservableObject
{
    [ObservableProperty]
    private string imagePath;
    private readonly ICameraService _cameraService;
    private readonly IPloggingSessionManager _ploggingSessionManager;

    public CheckoutImageViewModel(ICameraService cameraService, IPloggingSessionManager ploggingSessionManager)
    {
        _cameraService = cameraService;
        _ploggingSessionManager = ploggingSessionManager;
    }

    [RelayCommand]
    private async Task UsePhoto()
    {
        _ploggingSessionManager.SetPloggingSessionImage(ImagePath);
        _ploggingSessionManager.StopPlogging();

        WeakReferenceMessenger.Default.Send(new PhotoTakenMessage(ImagePath));

        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    [RelayCommand]
    private async Task RetakePhoto()
    {
        var imagePath = await _cameraService.TakePhoto();
        ImagePath = imagePath;
    }
}
