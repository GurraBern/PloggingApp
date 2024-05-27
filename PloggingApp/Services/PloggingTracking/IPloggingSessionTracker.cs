using PlogPal.Domain.Models;

namespace PloggingApp.Services.PloggingTracking;

public interface IPloggingSessionTracker
{
    void StartSession();
    Task EndSession(string imagePath);
    void AddLitterItem(LitterType litterType, double amount, Location location);
    Task AddTrashCollectionPoint(LitterbagPlacement litterbagPlacement);
    event EventHandler<Location> LocationUpdated;
}