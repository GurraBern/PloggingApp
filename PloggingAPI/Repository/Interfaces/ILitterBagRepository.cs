using Plogging.Core.Models;

namespace PloggingAPI.Services;

public interface ILitterbagRepository
{
    Task InsertLitterbagPlacement(LitterbagPlacement litterbagPlacement);
    Task<IEnumerable<LitterbagPlacement>> GetAllLitterbagPlacements();
    Task DeleteLitterbagPlacement(string litterbagPlacementId);
}