using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace PloggingApp.MVVM.Views;

public partial class DashboardView : ContentView
{
    int count = 0;

    public DashboardView()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
