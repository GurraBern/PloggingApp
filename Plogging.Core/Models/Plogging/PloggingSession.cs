using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Plogging.Core.Models;

public class PloggingSession
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

