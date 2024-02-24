using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Camera;

namespace PloggingApp.Pages;

[QueryProperty(nameof(ImagePath), nameof(ImagePath))]
public partial class CheckoutImageViewModel : ObservableObject
{
    [ObservableProperty]
    private string imagePath;
    private readonly ICameraService cameraService;

    public CheckoutImageViewModel(ICameraService cameraService)
    {
        this.cameraService = cameraService;
    }

    [RelayCommand]
    private async Task UsePhoto()
    {
        //TODO upload image alongside the plogging session data


        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    [RelayCommand]
    private async Task RetakePhoto()
    {
        var imagePath = await cameraService.TakePhoto();
        ImagePath = imagePath;
    }
}
