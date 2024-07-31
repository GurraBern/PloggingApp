using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PloggingApp.Features.PloggingSession;
using PlogPal.Application.Common.Interfaces;
using PlogPal.Domain.Enums;
using PlogPal.Domain.Models;
using PlogPal.Maui.Extensions;

namespace PlogPal.Maui.Features.PloggingSession;

public partial class AddLitterViewModel : ObservableObject, IRecipient<PloggingSessionMessage>
{
    private readonly IPloggingSessionManager _ploggingSessionManager;
    [ObservableProperty]
    private bool isTracking;
    //private Location CurrentLocation { get; set; }

    public AddLitterViewModel(IPloggingSessionManager ploggingSessionManager)
    {
        _ploggingSessionManager = ploggingSessionManager;

        WeakReferenceMessenger.Default.Register(this);
    }

    [RelayCommand]
    public void AddLitter(LitterType litterType)
    {
        //if (CurrentLocation != null)
        //{

        _ploggingSessionManager.AddLitter(litterType);

            //WeakReferenceMessenger.Default.Send(new LitterPlacedMessage(CurrentLocation.ToExternalLocation()));
        //}
    }

    public void Receive(PloggingSessionMessage message)
    {
        IsTracking = message.IsTracking;
    }
}
