using Microsoft.Maui.Maps;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Services.Authentication;

namespace PloggingApp.Services.PloggingTracking;

public class PloggingSessionTracker : IPloggingSessionTracker
{
    private readonly IPloggingSessionService _ploggingSessionService;
    private readonly ILitterBagPlacementService _litterBagPlacementService;
    private readonly IAuthenticationService _authenticationService;
    private const int DISTANCE_THRESHOLD = 20;
    private Task _updateSession;
    private List<Litter> CurrentLitter { get; set; } = [];
    private List<Location> Route { get; set; } = [];
    private DateTime StartTime { get; set; }
    public Location CurrentLocation { get; set; }
    public bool IsTracking { get; set; }
    public event EventHandler<Location> LocationUpdated;

    public PloggingSessionTracker(
        IPloggingSessionService ploggingSessionService,
        ILitterBagPlacementService litterBagPlacementService,
        IAuthenticationService authenticationService)
    {
        _ploggingSessionService = ploggingSessionService;
        _litterBagPlacementService = litterBagPlacementService;
        _authenticationService = authenticationService;
    }

    public void StartSession()
    {
        IsTracking = true;
        StartTime = DateTime.UtcNow;

        _updateSession = Task.Run(UpdateSession);
    }

    private async Task UpdateSession()
    {
        while(IsTracking)
        {
            CurrentLocation = await CurrentLocationAsync();

            if(CurrentLocation != null)
            {
                LocationUpdated?.Invoke(this, CurrentLocation);

                TrackRoute();

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }

    private async Task<Location> CurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var updatedLocation = await Geolocation.GetLocationAsync(request);

            if(updatedLocation != null)
            {
                return updatedLocation;
            } 
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task AddTrashCollectionPoint(LitterBagPlacement litterBagPlacement)
    {
        await _litterBagPlacementService.AddTrashCollectionPoint(litterBagPlacement);
    }

    public async Task EndSession()
    {
        IsTracking = false;

        //TODO remove below
        _authenticationService.CurrentUser.Info.DisplayName = "DisplayName";
        //

        var ploggingSession = new PloggingSession()
        {
            UserId = _authenticationService.CurrentUser.Uid,
            DisplayName = _authenticationService.CurrentUser.Info.DisplayName,
            StartDate = StartTime,
            EndDate = DateTime.UtcNow,
            PloggingData = new PloggingData()
            {
                Litters = CurrentLitter,
                Weight = CurrentLitter.Sum(x => x.Weight),
                Distance = CalculateTotalDistance(Route)
            } 
        };

        await _ploggingSessionService.SavePloggingSession(ploggingSession);
    }

    public void AddLitterItem(LitterType litterType, double amount, Location location)
    {
        if (location == null)
            return; //TODO show toast that not able to 

        var weight = LitterCalculator.CalculateWeight(litterType);
        var litterLocation = new MapPoint(location.Latitude, location.Longitude);
        var litter = new Litter(litterType, amount, litterLocation, weight);
        CurrentLitter.Add(litter);
    }

    private void TrackRoute()
    {
        var previousLocation = Route.LastOrDefault();
        if (previousLocation == null)
        {
            Route.Add(CurrentLocation);
            return;
        }

        var distance = Distance.BetweenPositions(CurrentLocation, new Location(previousLocation.Latitude, previousLocation.Longitude)).Meters;

        if (DISTANCE_THRESHOLD < distance)
        {
            Route.Add(CurrentLocation);
        }
    }

    private double CalculateTotalDistance(List<Location> locations)
    {
        double totalDistance = 0;

        for (int i = 0; i < locations.Count - 1; i++)
        {
            double distance = Distance.BetweenPositions(Route.ElementAt(i), locations.ElementAt(i + 1)).Meters;
            totalDistance += distance;
        }

        return Math.Round(totalDistance);
    }
}
