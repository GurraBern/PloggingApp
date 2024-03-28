using CommunityToolkit.Maui.Core;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.Services;

namespace PloggingApp.Pages;

public class MapPageViewModel
{
    public MapViewModel MapViewModel { get; set; }

    public MapPageViewModel(ILitterLocationService litterLocationService, ILitterbagPlacementService litterbagPlacementService, IPopupService popupService, IToastService toastService)
    {
        MapViewModel = new MapViewModel(litterLocationService, litterbagPlacementService, popupService, toastService);
    }
}
