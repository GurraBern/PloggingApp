using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages.Dashboard;

public partial class removeViewmodel
{
    public MapViewModel MapViewModel { get; set; }
    private readonly IPopupService popupService;

    public removeViewmodel(IPopupService popupService)
    {
        this.popupService = popupService;
        MapViewModel = new MapViewModel();
    }

    [RelayCommand]
    public async Task DisplayPopup()
    {
        await popupService.ShowPopupAsync<AcceptPopupViewModel>();
    }
}
