using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.Services.PloggingTracking;

namespace PloggingApp.Pages;

public partial class DashboardViewmodel
{
    public MapViewModel MapViewModel { get; set; }
    public PloggingSessionViewModel PloggingSessionViewModel{ get; set; }
    public AddLitterViewModel AddLitterViewModel { get; set; }
    private readonly IPopupService _popupService;

    public DashboardViewmodel(IPloggingSessionTracker ploggingSessionTracker, ILitterLocationService litterLocationService, IPopupService popupService)
    {
        _popupService = popupService;
        MapViewModel = new MapViewModel(litterLocationService);
        PloggingSessionViewModel = new PloggingSessionViewModel(ploggingSessionTracker);
        AddLitterViewModel = new AddLitterViewModel(ploggingSessionTracker);
    }

    [RelayCommand]
    public async Task DisplayPopup()
    {
        await _popupService.ShowPopupAsync<AcceptPopupViewModel>();
    }
}
