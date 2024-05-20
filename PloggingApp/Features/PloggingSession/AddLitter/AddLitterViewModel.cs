using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Enums;
using PloggingApp.Services.PloggingTracking;

namespace PloggingApp.Features.PloggingSession;

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
        if (CurrentLocation != null)
        {
            _ploggingSessionTracker.AddLitterItem(litterType, 1, CurrentLocation);

            WeakReferenceMessenger.Default.Send(new LitterPlacedMessage(CurrentLocation));
        }
    }

    public void Receive(PloggingSessionMessage message)
    {
        IsTracking = message.IsTracking;
    }
}
