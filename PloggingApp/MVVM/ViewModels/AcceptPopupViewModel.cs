using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PloggingApp.MVVM.ViewModels;

public partial class AcceptPopupViewModel : ObservableObject
{

    public AcceptPopupViewModel()
    {

    }

    [RelayCommand]
    private async Task NavigateToCameraView()
    {
        await Shell.Current.GoToAsync("CameraView");// TODO check if it can navigate to a view
    }
}