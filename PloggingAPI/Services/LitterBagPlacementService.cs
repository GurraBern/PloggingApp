using Plogging.Core.Models;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Services;

public class LitterBagPlacementService : ILitterBagPlacementService
{
    private readonly ILitterBagRepository _litterBagRepository;

    public LitterBagPlacementService(ILitterBagRepository litterBagRepository)
    {
        _litterBagRepository = litterBagRepository;
    }

    public async Task CreateLitterBagPlacement(LitterBagPlacement litterBagPlacement)
    {
        await _litterBagRepository.InsertLitterBagPlacement(litterBagPlacement);
    }

    public async Task<IEnumerable<LitterBagPlacement>> GetLitterBagPlacements()
    {
        var litterBagPlacements = await _litterBagRepository.GetAllLitterBagPlacements();

        return litterBagPlacements;
    }
}
