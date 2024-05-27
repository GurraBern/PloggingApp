using Plogging.Core.Models;

namespace Infrastructure.Services.Interfaces;

public interface ILitterLocationService
{
    Task<IEnumerable<LitterLocation>> GetLitterLocations();
}