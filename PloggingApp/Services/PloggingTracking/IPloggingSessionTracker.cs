using PlogPal.Domain.Models;
using Location = PlogPal.Domain.Models.Location;

namespace PloggingApp.Services.PloggingTracking;

public interface IPloggingSessionTracker
{
    void StartSession();
    Task EndSession(string imagePath);
    void AddLitterItem(LitterType litterType, double amount, Location location);
    Task AddTrashCollectionPoint(LitterbagPlacement litterbagPlacement);
    event EventHandler<Location> LocationUpdated;
}