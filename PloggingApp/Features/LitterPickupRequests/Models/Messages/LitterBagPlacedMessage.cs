using Plogging.Core.Models;

namespace PloggingApp.Features.LitterPickupRequests;

public class LitterBagPlacedMessage 
{
    public LitterBagPlacedMessage(LitterbagPlacement litterbagPlacement)
    {
        LitterbagPlacement = litterbagPlacement;
    }

    public LitterbagPlacement LitterbagPlacement { get; private set; }
}
