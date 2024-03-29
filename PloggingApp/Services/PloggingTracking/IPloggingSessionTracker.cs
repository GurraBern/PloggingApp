﻿using Plogging.Core.Enums;

namespace PloggingApp.Services.PloggingTracking;

public interface IPloggingSessionTracker
{
    void StartSession();
    Task EndSession();
    void AddLitterItem(LitterType litterType, double amount, Location location);
    Location CurrentLocation { get; set; }
    event EventHandler<Location> LocationUpdated;
}