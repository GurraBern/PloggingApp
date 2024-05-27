using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlogPal.Domain.Models;

public class PlogSession
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    public required string UserId { get; set; }
    public PloggingData PloggingData { get; set; } = new();
    public List<MapPoint> PloggingRoute { get; set; } = [];
    public string Image { get; set; }
}

