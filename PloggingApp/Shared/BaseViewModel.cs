using CommunityToolkit.Mvvm.ComponentModel;

namespace PlogPal.Maui.Shared;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;
}
