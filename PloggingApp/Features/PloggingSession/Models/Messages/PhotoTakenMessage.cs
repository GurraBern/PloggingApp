namespace PloggingApp.Features.PloggingSession;

public class PhotoTakenMessage
{
    public PhotoTakenMessage(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; set; }
}
