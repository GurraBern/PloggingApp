using Plogging.Core.Models;

namespace PloggingAPI.Services.Interfaces;

public interface ILitterBagPlacementService
{
    Task CreateLitterBagPlacement(LitterBagPlacement litterBagPlacement);
    Task<IEnumerable<LitterBagPlacement>> GetLitterBagPlacements();
}