namespace Plogging.Core.Models;

public class PloggingData
{
    public IEnumerable<Litter> Litters { get; set; } = [];
    public IEnumerable<GeoLocation> LitterLocations { get; set; } = [];
    public int Steps { get; set; }
    public double Distance { get; set; }
}
