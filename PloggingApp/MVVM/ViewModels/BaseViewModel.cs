using CommunityToolkit.Mvvm.ComponentModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    internal void OnAppearing()
    {
        throw new NotImplementedException();
    }
}
