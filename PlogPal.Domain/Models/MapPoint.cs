namespace PlogPal.Domain.Models;

public class MapPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;

    public MapPoint(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
