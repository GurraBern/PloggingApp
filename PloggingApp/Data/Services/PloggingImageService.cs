using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services;

public class PloggingImageService : IPloggingImageService
{
    private readonly IPloggingApiClient<PloggingImage> _imageApiClient;
    private readonly IAuthenticationService _authenticationService;

    public PloggingImageService(IPloggingApiClient<PloggingImage> imageApiClient, IAuthenticationService authenticationService)
    {
        _imageApiClient = imageApiClient;
        _authenticationService = authenticationService;
    }

    public async Task<PloggingImage> SaveImage(string imagePath)
    {
        var imageRequest = new RestRequest("api/Image");
        imageRequest.AddFile("image", imagePath);

        var ploggingImage = await _imageApiClient.PostAsync(imageRequest);

        return ploggingImage;
    }
}
