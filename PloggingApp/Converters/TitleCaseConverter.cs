
using System.Globalization;

namespace PloggingApp.Converters;
public class TitleCaseConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            string titleCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dateTime.ToString("dddd"));
            return $"{titleCase} {dateTime.ToString("dd/MM")}";
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
