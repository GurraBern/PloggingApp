using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public class MapPageViewModel
{
    public MapViewModel MapViewModel { get; set; }

    public MapPageViewModel(ILitterLocationService litterLocationService)
    {
        MapViewModel = new MapViewModel(litterLocationService);
    }
}
