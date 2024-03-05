using Plogging.Core.Models;
using System.Linq;

namespace PloggingApp.MVVM.Models;
public class Total
{
    public double month { get; set; }
    public double year { get; set; }
    public double allTime { get; set; }

    public Total()
    {
        month = default; year = default; allTime = default;
    }

    public Total(IEnumerable<PloggingSession> sessions, Func<PloggingSession, double> member)
    {
        month = CalculateSum(sessions.Where(s => s.StartDate > DateTime.UtcNow.AddMonths(-1)), member);
        year = CalculateSum(sessions.Where(s => s.StartDate > DateTime.UtcNow.AddYears(-1)), member);
        allTime = CalculateSum(sessions, member);
    }
    public static double CalculateSum(IEnumerable<PloggingSession> sessions, Func<PloggingSession, double> member)
    {
        return sessions.Sum(member);
    }
}
    