using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PlogPal.Domain.Models;

public class LitterbagPlacement
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public Location Location { get; set; }
    public DateTime PlacementDate { get; set; }
    public string ImageUrl {  get; set; }
}
