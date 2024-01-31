using PloggingApp.MVVM.Models;

namespace PloggingApp.Data.Context.Interfaces;

public interface IRankingContext
{
    IEnumerable<Ranking> Rankings { get; }
}
