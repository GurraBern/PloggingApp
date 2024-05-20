using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Extensions;

namespace PloggingApp.Archive;
public class Total<T>
{
    public T month { get; set; }
    public T year { get; set; }
    public T allTime { get; set; }

    public Total()
    {
        month = default; year = default; allTime = default;
    }

    public Total(IEnumerable<PlogSession> sessions, Func<PlogSession, T> member)
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
    public static T CalculateSum(IEnumerable<PlogSession> sessions, Func<PlogSession, T> member)
    {
        dynamic acc = default(T);
        foreach(PlogSession session in sessions)
        {
            acc += member(session);
        }
        return acc;
    }
}
    