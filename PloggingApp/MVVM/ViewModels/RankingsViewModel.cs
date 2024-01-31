using PloggingApp.Data.Context.Interfaces;
using PloggingApp.MVVM.Models;

namespace PloggingApp.MVVM.ViewModels;

public class RankingsViewModel
{
    public IEnumerable<Ranking> Rankings { get; }

    public RankingsViewModel(IRankingContext rankingContext)
    {
        Rankings = rankingContext.Rankings;
    }
}
