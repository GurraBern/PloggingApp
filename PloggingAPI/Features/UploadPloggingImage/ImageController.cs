using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlogPal.Domain.Models;

namespace PloggingAPI.Features.UploadPloggingImage;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly GoogleDriveService _googleDriveService;

    public ImageController(GoogleDriveService googleDriveService)
    {
        _googleDriveService = googleDriveService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateImage([FromForm] IFormFile image)
    {
        try
        {
            string imageUrl;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                imageUrl = await _googleDriveService.UploadImage(bytes, Guid.NewGuid() + ".jpeg");
            }

            var ploggingImage = new PloggingImage()
            {
                ImageUrl = imageUrl
            };

            return Ok(ploggingImage);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
