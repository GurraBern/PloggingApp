using RestSharp;

namespace PloggingApp.Data.Services.ApiClients;

public class PloggingApiClient<T>(IRestClient restClient) : IPloggingApiClient<T>
{
    private readonly IRestClient restClient = restClient;

    public async Task<T> GetAsync(RestRequest request, string bearerToken)
    {
        request.AddHeader("Authorization", $"Bearer {bearerToken}");
        return await restClient.GetAsync<T>(request);
    }

    public async Task<IEnumerable<T>> GetAllAsync(RestRequest request, string bearerToken = "")
    {
        request.AddHeader("Authorization", $"Bearer {bearerToken}");
        return await restClient.GetAsync<IEnumerable<T>>(request);
    }

    public async Task<T> PostAsync(RestRequest request, string bearerToken)
    {
        request.AddHeader("Authorization", $"Bearer {bearerToken}");
        return await restClient.PostAsync<T>(request);
    }
}

public interface IPloggingApiClient<T>
{
    Task<T> GetAsync(RestRequest request, string bearerToken = "");

    Task<IEnumerable<T>> GetAllAsync(RestRequest request, string bearerToken = "");

    Task<T> PostAsync(RestRequest request, string bearerToken = "");
}