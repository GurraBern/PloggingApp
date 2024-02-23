using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Pages;

namespace PloggingApp.MVVM.ViewModels;

public partial class AcceptPopupViewModel : ObservableObject
{
    private readonly IPopupService popupService;

    public AcceptPopupViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
    }

    [RelayCommand]
    private async Task NavigateToCameraView()
    {
        await Shell.Current.GoToAsync(nameof(CheckoutImagePage));
    }
}