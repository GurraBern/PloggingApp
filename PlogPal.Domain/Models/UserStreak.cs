using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlogPal.Domain.Models;

public class UserStreak
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string UserId { get; set; }
    public int Streak { get; set; }

    public int BiggestStreak { get; set; } 

    public DateTime LastPlogged { get; set; }
}

