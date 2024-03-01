using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Plogging.Core.Enums;

namespace Plogging.Core.Models;

public class Litter
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? PloggingSessionId { get; set; }

    public LitterType LitterType { get; set; }
    public double LitterCount { get; set; }
    public double Weight { get; set; }

    public Litter(LitterType litterType, double litterCount, double weight = 0)
    {
        LitterType = litterType;
        LitterCount = litterCount;
        Weight = weight;
    }
}
