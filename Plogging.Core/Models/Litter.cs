using Plogging.Core.Enums;

namespace Plogging.Core.Models;

public class Litter
{
    public LitterType LitterType { get; set; }
    public double Amount { get; set; }

    public Litter(LitterType litterType, double amount)
    {
        LitterType = litterType;
        Amount = amount;
    }
}
