using Plogging.Core.Enums;

namespace PloggingAPI.Models.Queries;

public class SessionSummaryQuery
{
    public SortDirection SortDirection { get; set; }
    public SortProperty SortProperty { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
