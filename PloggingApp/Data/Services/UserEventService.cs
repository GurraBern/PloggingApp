using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using RestSharp;

namespace PloggingApp.Data.Services;

public class UserEventService : IUserEventService
{
    private readonly IPloggingApiClient<UserEvent> _ploggingApiClient;

    public UserEventService(IPloggingApiClient<UserEvent> ploggingApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
    }

    public async Task CreateEvent(UserEvent userEvent)
    {
        var request = new RestRequest("api/UserEvents");
        request.AddBody(userEvent);

        await _ploggingApiClient.PostAsync(request);

    }

    public async Task<IEnumerable<UserEvent>> GetEvents()
    {
        var request = new RestRequest("api/UserEvents");

        var events = await _ploggingApiClient.GetAllAsync(request);

        return events;
    }
}
