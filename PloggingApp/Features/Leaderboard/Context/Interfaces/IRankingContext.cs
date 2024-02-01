using PloggingApp.Shared.Models;

namespace PloggingApp.Data.Context.Interfaces;

public interface IRankingContext
{
    IEnumerable<Ranking> Rankings { get; }
}
