using PlogPal.Domain.Models;

namespace PlogPal.Application.Common.Interfaces;

public interface ILitterLocationService
{
    Task<IEnumerable<LitterLocation>> GetLitterLocations();
}