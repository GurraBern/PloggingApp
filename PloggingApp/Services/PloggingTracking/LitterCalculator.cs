using Plogging.Core.Enums;

namespace PloggingApp.Services.PloggingTracking;

internal class LitterCalculator
{
    public static double CalculateWeight(LitterType litterType) => litterType switch
    {
        LitterType.Plastics => WeightConversion(3.5),
        LitterType.LargePlastics => WeightConversion(22.18),
        LitterType.Cigarette => WeightConversion(0.42),
        LitterType.SmallMetal => WeightConversion(7.22),
        LitterType.Cardboard => WeightConversion(9.94),
        LitterType.Fabric => WeightConversion(51.5),
        LitterType.Can => WeightConversion(14),
        _ => WeightConversion(0) 
    };

    private static double WeightConversion(double weight)
    {
        return weight / 1000;
    }
}
