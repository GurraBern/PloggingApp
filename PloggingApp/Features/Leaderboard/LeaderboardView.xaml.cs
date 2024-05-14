namespace PloggingApp.Features.Leaderboard;

public partial class LeaderboardView : ContentView
{
    public LeaderboardView()
    {
        InitializeComponent();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
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
