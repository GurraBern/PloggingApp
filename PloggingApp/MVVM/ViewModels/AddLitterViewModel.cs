using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Enums;
using PloggingApp.Services.PloggingTracking;

namespace PloggingApp.MVVM.ViewModels;

public partial class AddLitterViewModel
{
    private readonly IPloggingSessionTracker _ploggingSessionTracker;

    public AddLitterViewModel(IPloggingSessionTracker ploggingSessionTracker)
    {
        _ploggingSessionTracker = ploggingSessionTracker;
    }

    [RelayCommand]
    private void AddPlastic()
    {
        _ploggingSessionTracker.AddLitterItem(LitterType.Plastics, 1);
    }

    [RelayCommand]
    private void AddCan()
    {
        _ploggingSessionTracker.AddLitterItem(LitterType.Can, 1);
    }

    [RelayCommand]
    private void AddCigarette()
    {
        _ploggingSessionTracker.AddLitterItem(LitterType.Cigarette, 1);
    }
}
