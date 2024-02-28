using Plogging.Core.Models;
using PloggingApp.MVVM.ViewModels;
namespace PloggingApp.Pages;

public class OthersProfilePageViewModel
{
    public OthersSessionsViewModel OthersSessionsViewModel{ get; set; }

    public OthersProfilePageViewModel(UserRanking UserRank)
    {
        OthersSessionsViewModel = new OthersSessionsViewModel(UserRank);
    }
}
