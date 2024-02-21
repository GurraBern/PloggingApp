using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public class MapPageViewModel
{
    public MapViewModel MapViewModel { get; set; }
    public CameraViewModel CameraViewModel { get; set; }

    public MapPageViewModel()
    {
        MapViewModel = new MapViewModel();
        CameraViewModel = new CameraViewModel();
    }
}
