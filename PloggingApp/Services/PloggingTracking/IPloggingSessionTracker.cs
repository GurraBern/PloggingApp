﻿using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Services.PloggingTracking;

public interface IPloggingSessionTracker
{
    void StartSession();
    Task EndSession();
    void AddLitterItem(LitterType litterType, double amount, Location location);
    Task AddTrashCollectionPoint(LitterBagPlacement litterBagPlacement);
    event EventHandler<Location> LocationUpdated;
}