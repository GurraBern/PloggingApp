using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using PloggingAPI.Models;

namespace PloggingAPI.Features.UploadPloggingImage;

public class GoogleDriveService
{
    private readonly IOptions<GoogleDriveSettings> _options;
    private readonly DriveService _driveService;

    public GoogleDriveService(IOptions<GoogleDriveSettings> options)
    {
        _options = options;

        var credential = GoogleCredential.FromJson(_options.Value.ServiceAccount)
            .CreateScoped(DriveService.ScopeConstants.Drive);

        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });
    }

    public async Task<string> UploadImage(byte[] image, string fileName)
    {
        var fileMetaData = new Google.Apis.Drive.v3.Data.File()
        {
            Name = fileName,
            Parents = new List<string>() { _options.Value.DirectoryId }
        };

        await using var msSource = new MemoryStream(image);

        var request = _driveService.Files.Create(fileMetaData, msSource, "test/plain");
        request.Fields = "*";

        var results = await request.UploadAsync(CancellationToken.None);

        var uploadedFileId = request.ResponseBody?.Id;

        var fileUrl = $"https://drive.google.com/uc?export=view&id={uploadedFileId}";

        return fileUrl ?? string.Empty;
    }
}
