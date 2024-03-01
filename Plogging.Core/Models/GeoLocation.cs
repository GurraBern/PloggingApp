namespace Plogging.Core.Models;

public class GeoLocation
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;

    public GeoLocation(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}
