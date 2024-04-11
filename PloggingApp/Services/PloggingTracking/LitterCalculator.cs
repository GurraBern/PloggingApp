using Plogging.Core.Enums;

namespace PloggingApp.Services.PloggingTracking;

internal class LitterCalculator
{
    public static double CalculateWeight(LitterType litterType) => litterType switch
    {
        LitterType.Plastics => WeightConversion(4.52),
        LitterType.LargePlastics => WeightConversion(23.8),
        LitterType.Cigarette => WeightConversion(0.52),
        LitterType.SmallMetal => WeightConversion(3.82),
        LitterType.Cardboard => WeightConversion(10),
        LitterType.Fabric => WeightConversion(26),
        LitterType.Can => WeightConversion(14),
        _ => WeightConversion(0) 
    };

    private static double WeightConversion(double weight)
    {
        return weight / 1000;
    }
}
