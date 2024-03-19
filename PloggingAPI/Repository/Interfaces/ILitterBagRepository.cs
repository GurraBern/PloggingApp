using Plogging.Core.Models;

namespace PloggingAPI.Services;

public interface ILitterBagRepository
{
    Task InsertLitterBagPlacement(LitterBagPlacement litterBagPlacement);
    Task<IEnumerable<LitterBagPlacement>> GetAllLitterBagPlacements(); 
}