﻿//using CommunityToolkit.Mvvm.Messaging;
//using Microsoft.Maui.Maps;
//using PloggingApp.Features.Streak;
//using PloggingApp.Shared;
//using PlogPal.Domain.Models;
//using Location = Microsoft.Maui.Devices.Sensors.Location;

//namespace PloggingApp.Services.PloggingTracking;

//public class PloggingSessionTracker : IPloggingSessionTracker
//{
//    private readonly IPloggingSessionService _ploggingSessionService;
//    private readonly ILitterbagPlacementService _litterbagPlacementService;
//    private readonly IAuthenticationService _authenticationService;
//    private readonly IPlogTogetherService _plogTogetherService;
//    private readonly IUserInfoService _userInfoService;
//    private readonly IStreakService _streakService;
//    private readonly IToastService _toastService;
//    private const int DISTANCE_THRESHOLD = 20;
//    private Task _updateSession;
//    private List<Litter> CurrentLitter { get; set; } = [];
//    private List<Microsoft.Maui.Devices.Sensors.Location> Route { get; set; } = [];
//    private DateTime StartTime { get; set; }
//    public Microsoft.Maui.Devices.Sensors.Location CurrentLocation { get; set; }
//    public bool IsTracking { get; set; }
//    public event EventHandler<Location> LocationUpdated;

//    public PloggingSessionTracker(IPloggingSessionService ploggingSessionService,
//        ILitterbagPlacementService litterbagPlacementService,
//        IAuthenticationService authenticationService,
//        IPlogTogetherService plogTogetherService, 
//        IUserInfoService userInfoService,
//        IStreakService streakService,
//        IToastService toastService)
//    {
//        _ploggingSessionService = ploggingSessionService;
//        _litterbagPlacementService = litterbagPlacementService;
//        _authenticationService = authenticationService;
//        _plogTogetherService = plogTogetherService;
//        _userInfoService = userInfoService;
//        _streakService = streakService;
//        _toastService = toastService;
//    }

//    public void StartSession()
//    {
//        IsTracking = true;
//        StartTime = DateTime.UtcNow;

//        _updateSession = Task.Run(UpdateSession);
//    }

//    private async Task UpdateSession()
//    {
//        while(IsTracking)
//        {
//            CurrentLocation = await CurrentLocationAsync();

//            if(CurrentLocation != null)
//            {
//                LocationUpdated?.Invoke(this, CurrentLocation);

//                TrackRoute();

//                await Task.Delay(TimeSpan.FromSeconds(1));
//            }
//        }
//    }

//    private async Task<Microsoft.Maui.Devices.Sensors.Location> CurrentLocationAsync()
//    {
//        try
//        {
//            var request = new GeolocationRequest(GeolocationAccuracy.Best);
//            var updatedLocation = await Geolocation.GetLocationAsync(request);

//            if(updatedLocation != null)
//            {
//                return updatedLocation;
//            } 
//            else
//            {
//                return null;
//            }
//        }
//        catch (Exception)
//        {
//            return null;
//        }
//    }

//    public async Task AddTrashCollectionPoint(LitterbagPlacement litterBagPlacement)
//    {
//        await _litterbagPlacementService.AddTrashCollectionPoint(litterBagPlacement);
//    }

//    public async Task EndSession(string imagePath)
//    {
//        IsTracking = false;

//        var currentUserId = _authenticationService.UserId;

//        var userIsPloggingTogether = await _plogTogetherService.GetPlogTogether(currentUserId);

//        if (userIsPloggingTogether == null)
//        {
//            var ploggingSession = new PlogSession()
//            {
//                UserId = currentUserId,
//                DisplayName = _authenticationService.CurrentUser.Info.DisplayName,
//                StartDate = StartTime,
//                EndDate = DateTime.UtcNow,
//                PloggingRoute = PloggingSessionTracker.ConvertRoute(Route),
//                PloggingData = new PloggingData()
//                {
//                    Litters = CurrentLitter,
//                    Weight = CurrentLitter.Sum(x => x.Weight),
//                    Distance = CalculateTotalDistance(Route)
//                },
//                Image = imagePath
//            };

//            await _ploggingSessionService.SavePloggingSession(ploggingSession);
//            await _streakService.UpdateStreak(currentUserId);
//        }
//        else
//        {
//            List<string> usersInGroup = userIsPloggingTogether.UserIds;

//            DateTime EndTime = DateTime.UtcNow;

//            PloggingData ploggingData = new PloggingData()
//            {
//                Litters = CurrentLitter,
//                Weight = CurrentLitter.Sum(x => x.Weight),
//                Distance = CalculateTotalDistance(Route)
//            };

//            foreach (var userId in usersInGroup)
//            {
//                await _streakService.UpdateStreak(userId);

//                var user = await _userInfoService.GetUser(userId);
//                var displayName = user.DisplayName;

//                var ploggingSession = new PlogSession()
//                {
//                    UserId = userId,
//                    DisplayName = displayName,
//                    StartDate = StartTime,
//                    EndDate = EndTime,
//                    PloggingRoute = PloggingSessionTracker.ConvertRoute(Route),
//                    PloggingData = ploggingData,
//                    Image = imagePath
//                };

//                await _ploggingSessionService.SavePloggingSession(ploggingSession);
//            }
//            await _plogTogetherService.DeleteGroup(currentUserId);
//        }
//        var streakUser = await _streakService.GetUserStreak(currentUserId);

//        if(streakUser != null)
//        {
//            WeakReferenceMessenger.Default.Send(new UpdateStreakMessage(streakUser.Streak));
//            await _toastService.MakeToast("Plogging Session Completed!");
//        }
//    }

//    private static List<PlogPal.Domain.Models.Location> ConvertRoute(List<Microsoft.Maui.Devices.Sensors.Location> route)
//    {
//        var mapPoints = new List<PlogPal.Domain.Models.Location>();
//        foreach (var location in route)
//        {
//            var point = new PlogPal.Domain.Models.Location(location.Latitude, location.Longitude);
//            mapPoints.Add(point);
//        }

//        return mapPoints;
//    }

//    public void AddLitterItem(LitterType litterType, double amount, Microsoft.Maui.Devices.Sensors.Location location)
//    {
//        if (location == null)
//            return; //TODO show toast that not able to 

//        var weight = LitterCalculator.CalculateWeight(litterType);
//        var litterLocation = new PlogPal.Domain.Models.Location(location.Latitude, location.Longitude);
//        var litter = new Litter(litterType, amount, litterLocation, weight);
//        CurrentLitter.Add(litter);
//    }

//    private void TrackRoute()
//    {
//        var previousLocation = Route.LastOrDefault();
//        if (previousLocation == null)
//        {
//            Route.Add(CurrentLocation);
//            return;
//        }

//        var distance = Distance.BetweenPositions(CurrentLocation, new Microsoft.Maui.Devices.Sensors.Location(previousLocation.Latitude, previousLocation.Longitude)).Meters;

//        if (DISTANCE_THRESHOLD < distance)
//        {
//            Route.Add(CurrentLocation);
//        }
//    }

//    private double CalculateTotalDistance(List<Microsoft.Maui.Devices.Sensors.Location> locations)
//    {
//        double totalDistance = 0;

//        for (int i = 0; i < locations.Count - 1; i++)
//        {
//            double distance = Distance.BetweenPositions(Route.ElementAt(i), locations.ElementAt(i + 1)).Meters;
//            totalDistance += distance;
//        }

//        return Math.Round(totalDistance);
//    }
//}
