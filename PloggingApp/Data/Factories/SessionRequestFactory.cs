using Plogging.Core.Enums;
using RestSharp;

namespace PloggingApp.Data.Factories;

public class SessionRequestFactory
{
    public static RestRequest CreateRequest(DateTime startDate, DateTime endDate, SortDirection sortDirection, SortProperty sortProperty)
    {
        var request = new RestRequest("api/PloggingSession/Summary");
        request.AddParameter("startDate", startDate);
        request.AddParameter("endDate", endDate);
        request.AddParameter(nameof(SortDirection), sortDirection);
        request.AddParameter(nameof(SortProperty), sortProperty);

        return request;
    }
}
