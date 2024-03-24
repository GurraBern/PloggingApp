using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models.Messages;

public class LitterBagPlacedMessage 
{
    public LitterBagPlacedMessage(LitterbagPlacement litterbagPlacement)
    {
        LitterbagPlacement = litterbagPlacement;
    }

    public LitterbagPlacement LitterbagPlacement { get; private set; }
}
