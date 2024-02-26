using Plogging.Core.Enums;
using System.Globalization;

namespace PloggingApp.Converters;

public class SortPropertyToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        Enum.TryParse<SortProperty>(value.ToString(), out var checkValue);
        Enum.TryParse<SortProperty>(parameter.ToString(), out var targetValue);

        return checkValue.Equals(targetValue);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
