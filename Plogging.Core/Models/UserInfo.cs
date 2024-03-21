using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Plogging.Core.Models;

public class UserInfo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string UserId { get; set; }
    public required string DisplayName { get; set; }
}
