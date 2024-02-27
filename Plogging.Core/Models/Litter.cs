using Plogging.Core.Enums;

namespace Plogging.Core.Models;

public class Litter
{
    public LitterType LitterType { get; set; }
    public double LitterCount { get; set; }
    public double Weight { get; set; }

    public Litter(LitterType litterType, double litterCount, double weight = 0)
    {
        LitterType = litterType;
        LitterCount = litterCount;
        Weight = weight;
    }
}
