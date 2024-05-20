using Plogging.Core.Models;

namespace PloggingAPI.Features.LitterPickupRequests;

public class LitterbagPlacementService : ILitterbagPlacementService
{
    private readonly ILitterbagRepository _litterbagRepository;

    private const int LITTERBAG_DISTANCE_THRESHOLD = 40;
    public LitterbagPlacementService(ILitterbagRepository litterbagRepository)
    {
        _litterbagRepository = litterbagRepository;
    }

    public async Task CollectLitterbagPlacement(string litterbagPlacementId, int distanceToLitterbag)
    {
        if (distanceToLitterbag < LITTERBAG_DISTANCE_THRESHOLD)
        {
            await _litterbagRepository.DeleteLitterbagPlacement(litterbagPlacementId);
        }
    }

    public async Task CreateLitterbagPlacement(LitterbagPlacement litterbagPlacement)
    {
        await _litterbagRepository.InsertLitterbagPlacement(litterbagPlacement);
    }

    public async Task<IEnumerable<LitterbagPlacement>> GetLitterbagPlacements()
    {
        var litterbagPlacements = await _litterbagRepository.GetAllLitterbagPlacements();

        return litterbagPlacements;
    }
}
