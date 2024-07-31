using PlogPal.Domain.Models;

namespace PlogPal.Application.Common.Interfaces;

public interface ILocationTracker
{
    Task TrackLocation();

    ICollection<Location> PlogRoute { get; } 

    event EventHandler<Location> LocationUpdated;
}