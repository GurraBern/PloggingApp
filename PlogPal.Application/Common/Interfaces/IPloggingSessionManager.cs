using PlogPal.Domain.Enums;

namespace PlogPal.Application.Common.Interfaces;

public interface IPloggingSessionManager
{
    bool IsPlogging { get; }
    void StartPlogging();
    void StopPlogging();
    void AddLitter(LitterType litterType);
    void SetPloggingSessionImage(string imagePath);
}
