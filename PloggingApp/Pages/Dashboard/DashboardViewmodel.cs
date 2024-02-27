using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class DashboardViewmodel
{
    public MapViewModel MapViewModel { get; set; }
    private readonly IPopupService _popupService;

    public DashboardViewmodel(IPopupService popupService)
    {
        _popupService = popupService;
        MapViewModel = new MapViewModel();
    }

    [RelayCommand]
    public async Task DisplayPopup()
    {
        await _popupService.ShowPopupAsync<AcceptPopupViewModel>();
    }
}
