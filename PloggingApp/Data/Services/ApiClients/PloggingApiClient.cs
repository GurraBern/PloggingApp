using RestSharp;

namespace PloggingApp.Data.Services.ApiClients;

public class PloggingApiClient<T>(IRestClient restClient) : IPloggingApiClient<T>
{
    private readonly IRestClient restClient = restClient;

    public async Task<T> GetAsync(RestRequest request, string bearerToken)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            return await restClient.GetAsync<T>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(RestRequest request, string bearerToken = "")
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            return await restClient.GetAsync<IEnumerable<T>>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<T> PostAsync(RestRequest request, string bearerToken)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            return await restClient.PostAsync<T>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<T> DeleteAsync(RestRequest request, string bearerToken)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            return await restClient.DeleteAsync<T>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }
}

public interface IPloggingApiClient<T>
{
    Task<T> GetAsync(RestRequest request, string bearerToken = "");

    Task<IEnumerable<T>> GetAllAsync(RestRequest request, string bearerToken = "");

    Task<T> PostAsync(RestRequest request, string bearerToken = "");

    Task<T> DeleteAsync(RestRequest request, string bearerToken = "");
}