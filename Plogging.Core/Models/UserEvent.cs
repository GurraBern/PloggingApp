using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Plogging.Core.Attributes;

namespace Plogging.Core.Models;

public class UserEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [FutureDate]
    public DateTime StarDate { get; set; }

    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    public required string Title { get; set; }
    public string Description { get; set; }

    [Required(ErrorMessage = "Event must have a valid location"), ]
    public required MapPoint Location { get; set; }
    public int NumberOfParticipants { get; set; }
}
