namespace PloggingApp.Extensions;

public static class DateTimeExtensions
{
    public static DateTime FirstDateInMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    public static DateTime LastDateInMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    public static DateTime FirstDateInYear(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 1, 1);
    }

    public static DateTime LastDateInYear(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 12, 31);
    }
}
