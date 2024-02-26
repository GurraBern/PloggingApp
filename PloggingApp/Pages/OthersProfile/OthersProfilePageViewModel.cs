using PloggingApp.MVVM.ViewModels;
namespace PloggingApp.Pages;

public class OthersProfilePageViewModel
{
    public OthersSessionsViewModel OthersSessionsViewModel{ get; set; }

    public OthersProfilePageViewModel()
    {
        OthersSessionsViewModel = new OthersSessionsViewModel();
    }
}
