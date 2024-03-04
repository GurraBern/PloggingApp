using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models.Messages;

public class LitterPlacedMessage 
{
    public LitterPlacedMessage(Location location)
    {
        LitterLocation = new MapPoint(location.Latitude, location.Longitude);
    }

    public MapPoint LitterLocation { get; private set; }
}
