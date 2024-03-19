using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Plogging.Core.Models;

public class LitterBagPlacement
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public MapPoint Location { get; set; }
    public string Image {  get; set; }
    public string Description { get; set; }

    public LitterBagPlacement()
    {
        
    }
}
