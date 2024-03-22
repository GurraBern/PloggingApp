using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Models;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;
[QueryProperty(nameof(PloggingSession), nameof(PloggingSession))]
public partial class SessionStatisticsViewModel : BaseViewModel
{
    [ObservableProperty]
    PloggingSession ploggingSession;
    public SessionStatisticsViewModel()
    {
        
    }
}
