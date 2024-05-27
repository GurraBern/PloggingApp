﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlogPal.Domain.Models;

public class LitterLocation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public LitterType LitterType { get; set; }
    public double Weight { get; set; }
    public MapPoint Location { get; set; }

    public LitterLocation(LitterType litterType, double weight, MapPoint location)
    {
        LitterType = litterType;
        Weight = weight;
        Location = location;
    }

    public LitterLocation()
    {

    }
}