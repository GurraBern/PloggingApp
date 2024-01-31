using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class RankingsPage : ContentPage
{
    public RankingsPage(RankingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}