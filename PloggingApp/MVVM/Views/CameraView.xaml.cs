
namespace PloggingApp.MVVM.Views;

public partial class CameraView : ContentView
{
    public CameraView()
    {
        InitializeComponent();
    }

    private string capturedPhotoPath;
    private string savedPhotoPath;
    private async void TakePhotoButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                capturedPhotoPath = photo.FullPath;
            }
        } catch
        {

        }
    }

    

    }
