using MongoDB.Driver;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingAPI.Models.Queries;
using System.Linq.Expressions;
using SortDirection = Plogging.Core.Enums.SortDirection;

namespace PloggingAPI.Factories;

public static class SortDefinitionFactory
{
    public static SortDefinition<PloggingSession> CreateSortDefinition(SessionSummaryQuery query)
    {
        var sortProperty = query.SortProperty;
        return query.SortDirection switch
        {
            SortDirection.Descending => Builders<PloggingSession>.Sort.Descending(GetSortExpression(sortProperty)),
            SortDirection.Ascending => Builders<PloggingSession>.Sort.Ascending(GetSortExpression(sortProperty)),
            _ => Builders<PloggingSession>.Sort.Descending(GetSortExpression(sortProperty)),
        };
    }

    private static Expression<Func<PloggingSession, object>> GetSortExpression(SortProperty property)
    {
        return property switch
        {
            //SortProperty.ScrapCount => x => x.PloggingData.ScrapCount,
            SortProperty.Distance => x => x.PloggingData.Distance,
            SortProperty.Steps => x => x.PloggingData.Steps,
            _ => throw new ArgumentException("Invalid sort property", nameof(property))
        };
    }
}