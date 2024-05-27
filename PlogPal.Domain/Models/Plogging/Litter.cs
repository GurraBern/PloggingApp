namespace PlogPal.Domain.Models;

public class Litter
{
    public LitterType LitterType { get; set; }
    public double LitterCount { get; set; }
    public double Weight { get; set; }
    public MapPoint LitterLocation { get; set; }

    public Litter(LitterType litterType, double litterCount, MapPoint litterLocation, double weight = 0)
    {
        LitterType = litterType;
        LitterCount = litterCount;
        LitterLocation = litterLocation;
        Weight = weight;
    }
}
