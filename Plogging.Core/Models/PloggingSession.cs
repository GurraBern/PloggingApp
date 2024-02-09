using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Plogging.Core.Models;

public class PloggingSession
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement]
    public DateTime SessionDate { get; set; }
    public required string UserId { get; set; }
    public int ScrapCount { get; set; }
    public int Steps { get; set; }
    public double Distance { get; set; }
}
