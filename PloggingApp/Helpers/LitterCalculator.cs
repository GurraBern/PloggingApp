using Plogging.Core.Models;

namespace PloggingApp.Helpers;

public class LitterCalculator
{
    public static PloggingData CreatePloggingData(IEnumerable<Litter> litters)
    {
        var totalLitter = litters.GroupBy(l => l.LitterType)
            .Select(group =>
            {
                var totalAmount = group.Sum(l => l.LitterCount);
                var totalWeight = group.Sum(l => l.Weight);

                return new Litter(group.Key, totalAmount);
            });

        var ploggingData = new PloggingData()
        {
            Litters = totalLitter,
        };

        return ploggingData;
    }
}
