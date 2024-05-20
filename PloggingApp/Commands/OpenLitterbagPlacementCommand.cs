using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Features.LitterPickupRequests;
using System.ComponentModel;

namespace PloggingApp.Commands;

public class OpenLitterbagPlacementCommand : IAsyncRelayCommand
{
    private readonly IPopupService _popupService;
    private readonly LitterbagPlacement _litterbagPlacement;

    public Task? ExecutionTask => throw new NotImplementedException();

    public bool CanBeCanceled => throw new NotImplementedException();

    public bool IsCancellationRequested => throw new NotImplementedException();

    public bool IsRunning => throw new NotImplementedException();

    public event EventHandler? CanExecuteChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public OpenLitterbagPlacementCommand(IPopupService popupService, LitterbagPlacement litterbagPlacement)
    {
        _popupService = popupService;
        _litterbagPlacement = litterbagPlacement;
    }

    public async Task ExecuteAsync(object? parameter)
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Best);
        var currentLocation = await Geolocation.GetLocationAsync(request);

        await _popupService.ShowPopupAsync<LitterbagPlacementViewModel>(onPresenting: viewModel =>
        {
            viewModel.LitterbagPlacement = _litterbagPlacement;
            viewModel.CalculateDistance(currentLocation);
        });
    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }

    public void NotifyCanExecuteChanged()
    {
        throw new NotImplementedException();
    }

    public bool CanExecute(object? parameter)
    {
        throw new NotImplementedException();
    }

    public void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }
}
