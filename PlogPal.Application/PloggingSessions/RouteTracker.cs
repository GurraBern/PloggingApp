// using PlogPal.Application.Common.Interfaces;
// using PlogPal.Domain.Models;
//
// namespace PlogPal.Domain.Services;
//
// public class RouteTracker
// {
//     private readonly ILocationProvider _locationProvider;
//     public List<Location> Route { get; set; }
//
//     public RouteTracker(ILocationProvider locationProvider)
//     {
//         _locationProvider = locationProvider;
//         
//         locationTracker.LocationUpdated += LocationUpdated;
//     }
//
//     public void TrackRoute()
//     {
//         var previousLocation = Route.LastOrDefault();
//         if (previousLocation == null)
//         {
//             Route.Add(CurrentLocation);
//             return;
//         }
//
//         var distance = Distance.BetweenPositions(CurrentLocation, new Microsoft.Maui.Devices.Sensors.Location(previousLocation.Latitude, previousLocation.Longitude)).Meters;
//
//         if (DISTANCE_THRESHOLD < distance)
//         {
//             Route.Add(CurrentLocation);
//         }
//     }
//
//
//     private readonly List<Location> _route = new List<Location>();
//
//     public void LocationUpdated(object sender, Location location)
//     {
//         AddLocationToRoute(location);
//     }
// }
