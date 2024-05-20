using Plogging.Core.Enums;
using System.Globalization;

namespace PloggingApp.Converters;
class TimeResolutionToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is TimeResolution timeResolution)
        {
           if(timeResolution is TimeResolution.ThisYear)
            {
                if (parameter as string == "year")
                    return true;
                else
                    return false;
            }
            else
            {
                if ((parameter as string == "year"))
                    return false;
                else
                    return true;
            }
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
