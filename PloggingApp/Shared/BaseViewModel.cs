using CommunityToolkit.Mvvm.ComponentModel;

namespace PloggingApp.Shared;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;
}
