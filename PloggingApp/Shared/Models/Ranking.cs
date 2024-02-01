namespace PloggingApp.Shared.Models;

public class Ranking
{
    public required string DisplayName { get; set; }
    public int ScrapCount { get; set; }
    public int Steps { get; set; }
    public double Distance { get; set; }
}
