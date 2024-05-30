using PlogPal.Domain.Models;

namespace Infrastructure.Interfaces;

public interface IPloggingImageService
{
    Task<PloggingImage> SaveImage(string imagePath);
}
