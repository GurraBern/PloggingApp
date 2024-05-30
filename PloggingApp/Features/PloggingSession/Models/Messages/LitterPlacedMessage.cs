using PlogPal.Domain.Models;

namespace PloggingApp.Features.PloggingSession;

public class LitterPlacedMessage 
{
    public LitterPlacedMessage(Microsoft.Maui.Devices.Sensors.Location location)
    {
        LitterLocation = new PlogPal.Domain.Models.Location(location.Latitude, location.Longitude);
    }

    public PlogPal.Domain.Models.Location LitterLocation { get; private set; }
}
