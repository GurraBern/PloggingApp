using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Enums;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Services.PloggingTracking;

namespace PloggingApp.MVVM.ViewModels;

public partial class AddLitterViewModel : ObservableObject, IRecipient<PloggingSessionMessage>
{
    private readonly IPloggingSessionTracker _ploggingSessionTracker;

    [ObservableProperty]
    private bool isTracking;
    private Location CurrentLocation { get; set; }

    public AddLitterViewModel(IPloggingSessionTracker ploggingSessionTracker)
    {
        _ploggingSessionTracker = ploggingSessionTracker;

        _ploggingSessionTracker.LocationUpdated += OnLocationUpdated;

        WeakReferenceMessenger.Default.Register(this);
    }

    private void OnLocationUpdated(object? sender, Location location)
    {
        CurrentLocation = location;
    }

    [RelayCommand]
    public void AddLitter(LitterType litterType)
    {
        var currentLocation = _ploggingSessionTracker.CurrentLocation;

        if (currentLocation != null)
        {
            _ploggingSessionTracker.AddLitterItem(litterType, 1, currentLocation);

            WeakReferenceMessenger.Default.Send(new LitterPlacedMessage(currentLocation));
        }
    }

    public void Receive(PloggingSessionMessage message)
    {
        IsTracking = message.IsTracking;
    }
}
