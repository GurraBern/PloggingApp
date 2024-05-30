using Infrastructure.Interfaces;
using Infrastructure.Services.ApiClients;
using PlogPal.Domain.Models;
using RestSharp;

namespace Infrastructure.Services;

public class PloggingImageService(IPloggingApiClient<PloggingImage> imageApiClient) : IPloggingImageService
{
    private readonly IPloggingApiClient<PloggingImage> _imageApiClient = imageApiClient;

    public async Task<PloggingImage> SaveImage(string imagePath)
    {
        var imageRequest = new RestRequest("api/Image");
        imageRequest.AddFile("image", imagePath);

        var ploggingImage = await _imageApiClient.PostAsync(imageRequest);

        return ploggingImage;
    }
}
