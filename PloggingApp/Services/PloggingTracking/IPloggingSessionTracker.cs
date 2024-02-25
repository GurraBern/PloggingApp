namespace PloggingApp.Services.PloggingTracking;

public interface IPloggingSessionTracker
{
    void StartSession();
    Task EndSession();
}