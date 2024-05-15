using PloggingApp.Services.Authentication;
using RestSharp;

namespace PloggingApp.Shared;

public class PloggingApiClient<T>(IRestClient restClient, IAuthenticationService authService) : IPloggingApiClient<T>
{
    private readonly IRestClient _restClient = restClient;
    private readonly IAuthenticationService _authService = authService;

    public async Task<T> GetAsync(RestRequest request)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {_authService.BearerToken}");
            return await _restClient.GetAsync<T>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(RestRequest request)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {_authService.BearerToken}");
            return await _restClient.GetAsync<IEnumerable<T>>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<T> PostAsync(RestRequest request)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {_authService.BearerToken}");
            return await _restClient.PostAsync<T>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<T> DeleteAsync(RestRequest request)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {_authService.BearerToken}");
            return await _restClient.DeleteAsync<T>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<T> PatchAsync(RestRequest request)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {_authService.BearerToken}");
            return await _restClient.PatchAsync<T>(request);
        }
        catch (Exception ex)
        {
            //Add logging
            throw;
        }
    }

    public async Task<T> PutAsync(RestRequest request)
    {
        try
        {
            request.AddHeader("Authorization", $"Bearer {_authService.BearerToken}");
            return await _restClient.PutAsync<T>(request);
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
    Task<T> GetAsync(RestRequest request);

    Task<IEnumerable<T>> GetAllAsync(RestRequest request);

    Task<T> PostAsync(RestRequest request);

    Task<T> DeleteAsync(RestRequest request);

    Task<T> PatchAsync(RestRequest request);

    Task<T> PutAsync(RestRequest request);
}
