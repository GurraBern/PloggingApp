using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Plogging.Core.Models;

public class PlogTogether
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string OwnerUserId { get; set; }
    public required List<string> UserIds { get; set; }
}

