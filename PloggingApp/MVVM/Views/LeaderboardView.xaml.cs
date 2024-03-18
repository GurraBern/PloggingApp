using Microsoft.Maui.Controls;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class LeaderboardView : ContentView
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
}