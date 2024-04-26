using Plogging.Core.Models;

namespace PloggingApp.MVVM.Models.Messages;

public class LitterbagPickedUpMessage
{
    public LitterbagPickedUpMessage(LitterbagPlacement litterbagPlacement)
    {
        LitterbagPlacement = litterbagPlacement;
    }

    public LitterbagPlacement LitterbagPlacement { get; }
}
