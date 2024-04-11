using Android.Gms.Maps;

namespace PloggingApp;

class MapCallbackHandler(CustomMapHandler mapHandler) : Java.Lang.Object, IOnMapReadyCallback
{
    public void OnMapReady(GoogleMap googleMap)
    {
        mapHandler.UpdateValue(nameof(Microsoft.Maui.Maps.IMap.Pins));
        mapHandler.Map?.SetOnMarkerClickListener(new CustomMarkerClickListener(mapHandler));
        mapHandler.Map?.SetOnInfoWindowClickListener(new CustomInfoWindowClickListener(mapHandler));

        if(mapHandler.Map != null)
        {
            mapHandler.Map.UiSettings.ZoomControlsEnabled = false;
        }
    }
}
