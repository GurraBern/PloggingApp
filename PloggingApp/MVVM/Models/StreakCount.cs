using CommunityToolkit.Mvvm.ComponentModel;

namespace PloggingApp.MVVM.Models;

public partial class StreakCount : ObservableObject
{
    [ObservableProperty]
    public int count;
}
