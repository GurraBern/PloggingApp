using Android.Gms.Maps.Model;
using static Android.Gms.Maps.GoogleMap;
namespace PloggingApp;


internal class CustomMarkerClickListener(CustomMapHandler mapHandler)
    : Java.Lang.Object, IOnMarkerClickListener
{
    public bool OnMarkerClick(Marker marker)
    {
        var pin = mapHandler.Markers.FirstOrDefault(x => x.marker.Id == marker.Id);
        pin.pin?.SendMarkerClick();
        marker.ShowInfoWindow();
        return true;
    }
}