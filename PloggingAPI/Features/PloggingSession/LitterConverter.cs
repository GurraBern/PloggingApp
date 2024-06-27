﻿using PlogPal.Domain.Models;

namespace PloggingAPI.Features.PloggingSession;

public class LitterConverter
{
    public static IEnumerable<LitterLocation> ToLitterLocations(IEnumerable<Litter> litters)
    {
        var litterLocations = new List<LitterLocation>();

        foreach (var litter in litters)
        {
            var litterLocation = new LitterLocation()
            {
                LitterType = litter.LitterType,
                Weight = litter.Weight,
                Location = litter.LitterLocation
            };

            litterLocations.Add(litterLocation);
        }

        return litterLocations;
    }
}
