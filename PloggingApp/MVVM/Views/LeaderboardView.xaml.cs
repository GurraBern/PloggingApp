
using Microsoft.Maui.Platform;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class LeaderboardView : Microsoft.Maui.Controls.ContentView
{
    public LeaderboardView()
    {
        InitializeComponent();
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var vm = (LeaderboardViewModel) BindingContext;

        if(vm != null && searchBar.Text.Length == 0)
        {
            vm.SearchUsers(searchBar.Text);
        }
    }

    private void GridTap(object sender, EventArgs e)
    {
        if (sender is Grid tappedGrid)
        {
            tappedGrid.Opacity = 0.5;

            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
            {
                tappedGrid.Opacity = 1;
                return false;
            });
        }
    }
}
