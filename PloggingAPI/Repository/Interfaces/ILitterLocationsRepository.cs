using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface ILitterLocationsRepository
{
    Task InsertLitterLocations(IEnumerable<GeoLocation> litterLocations);
}