using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces
{
    public interface IPloggingSessionService
    {
        Task SavePloggingSession(PloggingSession ploggingSession);
    }
}