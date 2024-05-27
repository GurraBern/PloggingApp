using Plogging.Core.Models;

namespace Infrastructure.Services.Interfaces;

public interface IPloggingImageService
{
    Task<PloggingImage> SaveImage(string imagePath);
}
