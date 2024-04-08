using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class SessionStatsMapViewModel : ObservableObject
{
    public ObservableCollection<LocationPin> TrashPins { get; set; } = [];
    public ObservableCollection<MapPoint> LocationPins { get; set; } = [];

    public PloggingSession PloggingSession { get; set; }

    public SessionStatsMapViewModel()
    {

    }
    public void Initialize(PloggingSession ploggingSession)
    {
        PloggingSession = ploggingSession;
        PlaceTrashPins();
    }
    private void PlaceTrashPins()
    {
        foreach(var location in PloggingSession.PloggingData.Litters) 
        {
            TrashPins.Add(new CanPin()
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
