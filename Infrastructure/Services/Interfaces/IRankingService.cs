﻿using Plogging.Core.Enums;
using PlogPal.Domain.Models;

namespace Infrastructure.Services.Interfaces;

public interface IRankingService
{
    Task InitializeAsync();
    Task<IEnumerable<UserRanking>> GetUserRankings(DateTime startDate, DateTime endDate, SortProperty sortProperty = SortProperty.Weight);
    public IEnumerable<UserRanking> UserRankings { get; }
    UserRanking UserRank { get; }
}