using PlogPal.Domain.Models;

namespace PloggingApp.Features.LitterPickupRequests;

public class LitterBagPlacedMessage 
{
    public LitterBagPlacedMessage(LitterbagPlacement litterbagPlacement)
    {
        LitterbagPlacement = litterbagPlacement;
    }

    public LitterbagPlacement LitterbagPlacement { get; private set; }
}
