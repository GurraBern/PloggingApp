using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlogPal.Domain.Models;

public class UserRanking
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string DisplayName { get; set; }
    public int Rank { get; set; }
    public PloggingData PloggingData { get; set; } = new();

    public static UserRanking CreateDefault()
    {
        return new UserRanking()
        {
            Id = "",
            DisplayName = "",
            Rank = 0,
            PloggingData = new PloggingData()
        };
    }
}
