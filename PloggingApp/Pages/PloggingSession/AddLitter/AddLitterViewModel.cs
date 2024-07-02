using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PloggingApp.Features.PloggingSession;
using PloggingApp.Services.PloggingTracking;
using PlogPal.Domain.Models;
using PlogPal.Maui.Extensions;
using Location = PlogPal.Domain.Models.Location;

namespace PlogPal.Maui.Features.PloggingSession;

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

            WeakReferenceMessenger.Default.Send(new LitterPlacedMessage(CurrentLocation.ToExternalLocation()));
        }
    }

    public void Receive(PloggingSessionMessage message)
    {
        IsTracking = message.IsTracking;
    }
}
