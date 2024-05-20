using System.Globalization;

namespace PloggingApp.Converters;
public class IntToMonthConverter : IValueConverter
{
    private static readonly List<string> months =
    [
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ];

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value == null) return null;
        return months[(int)value];
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
