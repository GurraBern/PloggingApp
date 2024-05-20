using Plogging.Core.Models;

namespace PloggingAPI.Features.LitterPickupRequests;

public interface ILitterbagPlacementService
{
    Task CollectLitterbagPlacement(string litterbagPlacementId, int distanceToLitterbag);
    Task CreateLitterbagPlacement(LitterbagPlacement litterbagPlacement);
    Task<IEnumerable<LitterbagPlacement>> GetLitterbagPlacements();
}