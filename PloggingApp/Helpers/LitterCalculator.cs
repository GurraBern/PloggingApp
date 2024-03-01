using Plogging.Core.Models;
using PloggingApp.Extensions;

namespace PloggingApp.Helpers;

public class LitterCalculator
{
    public static PloggingData CreatePloggingData(IEnumerable<Litter> litters, IEnumerable<Location> locations)
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
            LitterLocations = locations.ToLitterLocations()
        };

        return ploggingData;
    }
}
