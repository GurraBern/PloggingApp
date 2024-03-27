using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Extensions;
using System.Linq;

namespace PloggingApp.MVVM.Models;
public class Total<T>
{
    public T month { get; set; }
    public T year { get; set; }
    public T allTime { get; set; }

    public Total()
    {
        month = default; year = default; allTime = default;
    }

    public Total(IEnumerable<PloggingSession> sessions, Func<PloggingSession, T> member)
    {
        month = CalculateSum(sessions.Where(s => s.StartDate > DateTime.UtcNow.FirstDateInMonth()), member);
        year = CalculateSum(sessions.Where(s => s.StartDate > DateTime.UtcNow.FirstDateInYear()), member);
        allTime = CalculateSum(sessions, member);
    }
    public T GetValue(TimeResolution tr)
    {
        switch (tr)
        {
            case TimeResolution.ThisMonth: return month;
            case TimeResolution.Alltime: return allTime;
            default: return year;
        }
    }
    public static T CalculateSum(IEnumerable<PloggingSession> sessions, Func<PloggingSession, T> member)
    {
        dynamic acc = default(T);
        foreach(PloggingSession session in sessions)
        {
            acc += member(session);
        }
        return acc;
    }
}
    