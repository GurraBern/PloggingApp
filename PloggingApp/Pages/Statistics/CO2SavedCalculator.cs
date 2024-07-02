using PlogPal.Domain.Enums;
using PlogPal.Domain.Models;

namespace PloggingApp.Features.Statistics;
// Values are based on the findings in "Greenhouse gas emission factors for recycling of source-segregated waste materials"
// by David A. Turner et al. @ Table 6.
public class CO2SavedCalculator
{
    private static Dictionary<LitterType, double> CO2PerKgPairs = new Dictionary<LitterType, double>()
    {
        {LitterType.Plastics, 1.024}, //Mixed Plastics
        {LitterType.LargePlastics, 1.024 },
        {LitterType.Cigarette, 0},// Cellulose acetate fibres = plastic (?)
        {LitterType.SmallMetal, 3.577 }, // Other scrap metal
        {LitterType.Cardboard, 0.452 }, // Composite food & beverage cans. (Alt. paper(?))
        {LitterType.Can, 8.143}, // Aluminium cans
        {LitterType.Fabric, 3.376}, // Textiles only
        {LitterType.Snus, 0}, // ?? 
        {LitterType.Glass, 0.314 } // Mixed glass
    };

    public static double CalculateCO2Saved(Litter litter)
    {
        return (CO2PerKgPairs[litter.LitterType]*(litter.Weight));
    }
    public static double CalculateCO2Saved(IEnumerable<PlogSession> sessions)
    {
        return Calculate(sessions.SelectMany(s => s.PloggingData.Litters).ToList());
    }
    public static double CalculateCO2Saved(PlogSession session)
    {
        return Calculate(session.PloggingData.Litters);
    }
    private static double Calculate(List<Litter> litters)
    {
        return litters.Sum(l => (CO2PerKgPairs[l.LitterType]) * l.Weight);
    }
    

}
