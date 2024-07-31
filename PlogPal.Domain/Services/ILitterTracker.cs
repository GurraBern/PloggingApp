using PlogPal.Domain.Enums;
using PlogPal.Domain.Models;

namespace PlogPal.Application.Common.Interfaces;

public interface ILitterTracker
{
    void AddLitter(LitterType litterType, Location location);

    ICollection<Litter> Litters { get; }
}
