using Plogging.Core.Enums;

namespace PloggingApp.Extensions;

public static class UnitExtensions
{
    public static string GetUnitOfMeasurement(this SortProperty sortProperty)
    {
        return sortProperty switch
        {
            SortProperty.Weight => "kg",
            SortProperty.Distance => "meters",
            SortProperty.Steps => "steps",
            _ => ""
        };
    }
}
