namespace PloggingApp.Services.Camera;

public interface ICameraService
{
    Task<string> TakePhoto();
}
