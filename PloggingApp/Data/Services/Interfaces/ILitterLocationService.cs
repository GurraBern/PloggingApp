using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces;

public interface ILitterLocationService
{
    Task<IEnumerable<LitterLocation>> GetLitterLocations();
}