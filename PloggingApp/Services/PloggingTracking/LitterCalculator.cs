using Plogging.Core.Enums;

namespace PloggingApp.Services.PloggingTracking;

internal class LitterCalculator
{
    public static double CalculateWeight(LitterType litterType) => litterType switch
    {
        LitterType.Plastics => 4.52,
        LitterType.LargePlastics => 23.8,
        LitterType.Cigarette => 0.52,
        LitterType.SmallMetal => 3.82,
        LitterType.Cardboard => 10,
        LitterType.Fabric => 26,
        LitterType.Can => 14,
        _ => 0 
    };
}
