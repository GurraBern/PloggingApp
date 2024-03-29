﻿using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PloggingApp.MVVM.Models.Messages;

public class PloggingSessionMessage : ValueChangedMessage<bool>
{
    public PloggingSessionMessage(bool isTracking, List<Location> locations) : base(isTracking)
    {
        IsTracking = isTracking;
        Locations = locations;
    }

    public bool IsTracking { get; set; }
    public List<Location> Locations { get; set; }
}
