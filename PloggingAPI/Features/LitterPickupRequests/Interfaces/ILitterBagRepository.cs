
using PlogPal.Domain.Models;

namespace PloggingAPI.Features.LitterPickupRequests;

public interface ILitterbagRepository
{
    Task InsertLitterbagPlacement(LitterbagPlacement litterbagPlacement);
    Task<IEnumerable<LitterbagPlacement>> GetAllLitterbagPlacements();
    Task DeleteLitterbagPlacement(string litterbagPlacementId);
}