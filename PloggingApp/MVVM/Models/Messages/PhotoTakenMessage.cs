namespace PloggingApp.MVVM.Models.Messages;

public class PhotoTakenMessage
{
    public PhotoTakenMessage(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; set; }
}
