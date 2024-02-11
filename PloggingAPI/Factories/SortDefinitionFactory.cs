using MongoDB.Driver;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingAPI.Models.Queries;
using SortDirection = Plogging.Core.Enums.SortDirection;

namespace PloggingAPI.Factories;

public static class SortDefinitionFactory
{
    public static SortDefinition<PloggingSession> CreateSortDefinition(SessionSummaryQuery query)
    {
        var sortProperty = query.SortProperty;
        return query.SortDirection switch
        {
            SortDirection.Descending => Builders<PloggingSession>.Sort.Descending(GetPropertyName(sortProperty)),
            SortDirection.Ascending => Builders<PloggingSession>.Sort.Ascending(GetPropertyName(sortProperty)),
            _ => Builders<PloggingSession>.Sort.Descending(GetPropertyName(sortProperty)),
        };
    }

    private static string GetPropertyName(SortProperty property)
    {
        return property switch
        {
            SortProperty.ScrapCount => nameof(PloggingData.ScrapCount),
            SortProperty.Distance => nameof(PloggingData.Distance),
            SortProperty.Steps => nameof(PloggingData.Steps),
            _ => throw new ArgumentException("Invalid sort property", nameof(property))
        };
    }
}