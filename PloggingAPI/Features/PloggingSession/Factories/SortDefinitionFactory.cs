using MongoDB.Driver;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using System.Linq.Expressions;
using SortDirection = Plogging.Core.Enums.SortDirection;

namespace PloggingAPI.Features.PloggingSession;

public static class SortDefinitionFactory
{
    public static SortDefinition<PlogSession> CreateSortDefinition(SessionSummaryQuery query)
    {
        var sortProperty = query.SortProperty;
        return query.SortDirection switch
        {
            SortDirection.Descending => Builders<PlogSession>.Sort.Descending(GetSortExpression(sortProperty)),
            SortDirection.Ascending => Builders<PlogSession>.Sort.Ascending(GetSortExpression(sortProperty)),
            _ => Builders<PlogSession>.Sort.Descending(GetSortExpression(sortProperty)),
        };
    }

    private static Expression<Func<PlogSession, object>> GetSortExpression(SortProperty property)
    {
        return property switch
        {
            SortProperty.Weight => x => x.PloggingData.Weight,
            SortProperty.Distance => x => x.PloggingData.Distance,
            SortProperty.Steps => x => x.PloggingData.Steps,
            _ => throw new ArgumentException("Invalid sort property", nameof(property))
        };
    }
}