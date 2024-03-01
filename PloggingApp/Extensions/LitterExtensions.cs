using Plogging.Core.Models;

namespace PloggingApp.Extensions;

public static class LitterExtensions
{
    public static IEnumerable<GeoLocation> ToLitterLocations(this IEnumerable<Location> locations)
    {
        var litterLocations = new List<GeoLocation>();
        foreach (var location in locations)
        {
            litterLocations.Add(new GeoLocation(location.Longitude, location.Latitude));
        }

        return litterLocations;
    }
}
