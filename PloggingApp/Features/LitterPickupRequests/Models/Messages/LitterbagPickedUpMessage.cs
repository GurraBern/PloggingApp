using Plogging.Core.Models;

namespace PloggingApp.Features.LitterPickupRequests;

public class LitterbagPickedUpMessage
{
    public LitterbagPickedUpMessage(LitterbagPlacement litterbagPlacement)
    {
        LitterbagPlacement = litterbagPlacement;
    }

    public LitterbagPlacement LitterbagPlacement { get; }
}
