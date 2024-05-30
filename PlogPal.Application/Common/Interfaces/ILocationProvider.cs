using PlogPal.Domain.Models;

namespace PlogPal.Application.Common.Interfaces;

public interface ILocationProvider
{
    Task<Location> GetCurrentLocation();
}