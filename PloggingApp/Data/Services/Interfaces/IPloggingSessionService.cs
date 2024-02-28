using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces
{
    public interface IPloggingSessionService
    {
        Task SavePloggingSession(PloggingSession ploggingSession);

        Task<IEnumerable<PloggingSession>> GetUserSessions(DateTime startDate, DateTime endDate, SortProperty sortProperty);
    }
}