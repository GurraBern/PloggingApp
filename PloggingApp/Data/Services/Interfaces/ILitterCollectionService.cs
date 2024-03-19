using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces;

public interface ILitterBagPlacementService 
{
    Task AddTrashCollectionPoint(LitterBagPlacement litterBagPlacement);
    Task<IEnumerable<LitterBagPlacement>> GetLitterBagPlacements();
}