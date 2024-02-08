using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Plogging.Core.Models;

public class UserRanking
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string DisplayName { get; set; }

    public int Rank { get; set; }

    [BsonElement]
    public int ScrapCount { get; set; }
    public int Steps { get; set; }
    public double Distance { get; set; }
}
