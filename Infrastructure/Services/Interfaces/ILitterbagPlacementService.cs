using PlogPal.Domain.Models;

namespace Infrastructure.Services.Interfaces;

public interface ILitterbagPlacementService
{
    Task AddTrashCollectionPoint(LitterbagPlacement litterbagPlacement);
    Task CollectLitterbagPlacement(string id, int distance);
    Task<IEnumerable<LitterbagPlacement>> GetLitterbagPlacements();
}