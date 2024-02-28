using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Plogging.Core.Models;

public class PloggingSession
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;

    public byte[] SessionImage { get; set; }
    public required string UserId { get; set; }
    public required string DisplayName { get; set; }
    public PloggingData PloggingData { get; set; } = new();
}

