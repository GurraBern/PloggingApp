using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Plogging.Core.Models;
using PloggingApp.Features.Map.Components;
using PloggingApp.MVVM.Models.Messages;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class SessionStatsMapViewModel : ObservableObject
{ 
    public ObservableCollection<LocationPin> TrashPins { get; set; } = [];
    public ObservableCollection<MapPoint> LocationPins { get; set; } = [];



    [ObservableProperty]
    private PloggingSession ploggingSession;

    public SessionStatsMapViewModel()
    {

    }
    public void Initialize(PloggingSession ploggingSession)
     {
        PloggingSession = ploggingSession;
        PlaceTrashPins();
        LocationPins = new ObservableCollection<MapPoint>(ploggingSession.PloggingRoute);
        WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(true, []));
    }

    private void PlaceTrashPins()
    {
        foreach(var location in PloggingSession.PloggingData.Litters) 
        {
            TrashPins.Add(new CollectedLitterPin()
            {
                Label = "Litter",
                Location = new Location()
                {
                    Latitude = location.LitterLocation.Latitude,
                    Longitude = location.LitterLocation.Longitude
                }
            });
        }
    }

}
