﻿using PlogPal.Domain.Models;

namespace PloggingAPI.Features.LitterLocations;

public interface ILitterLocationsRepository
{
    Task InsertLitterLocations(IEnumerable<LitterLocation> litterLocations);
    Task<IEnumerable<LitterLocation>> GetLitterLocations();
}