using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;

namespace PloggingApp.MVVM.Views;

public partial class MapView : ContentView
{
    public MapView()
    {
        InitializeComponent();

        //TODO C
        ploggingMap.MoveToRegion(new MapSpan(new Location(57.7, 11.96), 10, 15));
    }
}
