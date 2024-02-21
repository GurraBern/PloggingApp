using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PloggingApp.MVVM.ViewModels
{
    public partial class CameraViewModel
    {

        public ImageSource PhotoTaken { get; set; }


        [RelayCommand]
        private async void TakePhoto()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    string capturedPhotoPath = photo.FullPath;
                    PhotoTaken = ImageSource.FromFile(capturedPhotoPath);
                }
            }
            catch (PermissionException ex)
            {
                // Handle permission-related exception
                Console.WriteLine($"Permission Exception: {ex.Message}");
                // Possibly prompt the user to grant necessary permissions
            }
            catch (TaskCanceledException ex)
            {
                // Handle task cancellation
                Console.WriteLine($"Task Canceled Exception: {ex.Message}");
                // Possibly inform the user about the cancellation
            }
            catch (FileNotFoundException ex)
            {
                // Handle file not found exception
                Console.WriteLine($"File Not Found Exception: {ex.Message}");
                // Possibly inform the user about the issue
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                // Possibly log the exception or inform the user about the issue
            }


















        }
    }
}
