using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Models;

namespace PloggingApp.MVVM.ViewModels;

public partial class LitterBagPlacementViewModel : ObservableObject
{
    [ObservableProperty]
    public LitterBagPlacement litterBagPlacement = new();
}
