using Plogging.Core.Enums;

namespace PloggingApp.Services.PloggingTracking;

public interface IPloggingSessionTracker
{
    void StartSession();
    Task EndSession();
    void AddLitterItem(LitterType litterType, double amount);
}