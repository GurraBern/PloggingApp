namespace PlogPal.Application.Common.Interfaces;

public interface IPloggingSessionManager
{
    bool IsPlogging { get; }
    void StartPlogging();
    void StopPlogging();
}
