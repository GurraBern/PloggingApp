using PlogPal.Domain.Models;

namespace PloggingApp.Features.PloggingSession;

public class LitterPlacedMessage 
{
    public LitterPlacedMessage(Location location)
    {
        LitterLocation = new MapPoint(location.Latitude, location.Longitude);
    }

    public MapPoint LitterLocation { get; private set; }
}
