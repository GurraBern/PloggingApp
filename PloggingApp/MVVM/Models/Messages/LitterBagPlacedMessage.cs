using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models.Messages;

public class LitterBagPlacedMessage 
{
    public LitterBagPlacedMessage(LitterBagPlacement litterBagPlacement)
    {
        LitterBagPlacement = litterBagPlacement;
    }

    public LitterBagPlacement LitterBagPlacement { get; private set; }
}
