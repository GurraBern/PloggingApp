
using PlogPal.Common.Enums;

namespace PloggingAPI.Features.PloggingSession;

public class SessionSummaryQuery
{
    public SortDirection SortDirection { get; set; }
    public SortProperty SortProperty { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
