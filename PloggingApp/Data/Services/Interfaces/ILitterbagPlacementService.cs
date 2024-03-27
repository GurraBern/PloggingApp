using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces;

public interface ILitterbagPlacementService 
{
    Task AddTrashCollectionPoint(LitterbagPlacement litterbagPlacement);
    Task CollectLitterbagPlacement(string id, int distance);
    Task<IEnumerable<LitterbagPlacement>> GetLitterbagPlacements();
}