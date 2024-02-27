using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class DashboardViewmodel
{
    public MapViewModel MapViewModel { get; set; }
    public PloggingSessionViewModel PloggingSessionViewModel{ get; set; }
    private readonly IPopupService _popupService;

    public DashboardViewmodel(IPopupService popupService)
    {
        _popupService = popupService;
        MapViewModel = new MapViewModel();
        PloggingSessionViewModel = new PloggingSessionViewModel();
    }

    [RelayCommand]
    public async Task DisplayPopup()
    {
        await _popupService.ShowPopupAsync<AcceptPopupViewModel>();
    }
}
