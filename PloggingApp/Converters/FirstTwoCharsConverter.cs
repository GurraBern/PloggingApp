using System.Globalization;

namespace PloggingApp.Converters;

public class FirstTwoCharsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = value as string;
        return str?.Substring(0, Math.Min(2, str.Length)) ?? "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
