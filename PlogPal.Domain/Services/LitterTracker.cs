using PlogPal.Application.Common.Interfaces;
using PlogPal.Domain.Enums;
using PlogPal.Domain.Models;

namespace PlogPal.Domain.Services;

public class LitterTracker : ILitterTracker
{
    public ICollection<Litter> Litters { get; } = [];

    public void AddLitter(LitterType litterType, Location location)
    {
        var litter = new Litter(litterType, 1, location);
        Litters.Add(litter);
    }
}
