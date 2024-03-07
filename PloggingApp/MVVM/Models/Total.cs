using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Extensions;
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
        month = CalculateSum(sessions.Where(s => s.StartDate > DateTime.UtcNow.FirstDateInMonth()), member);
        year = CalculateSum(sessions.Where(s => s.StartDate > DateTime.UtcNow.FirstDateInYear()), member);
        allTime = CalculateSum(sessions, member);
    }
    public double GetValue(TimeResolution tr)
    {
        switch (tr)
        {
            case TimeResolution.ThisMonth: return month;
            case TimeResolution.Alltime: return allTime;
            default: return year;
        }
    }
    public static double CalculateSum(IEnumerable<PloggingSession> sessions, Func<PloggingSession, double> member)
    {
        return sessions.Sum(member);
    }
}
    