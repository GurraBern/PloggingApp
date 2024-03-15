using PloggingApp.MVVM.ViewModels;
using PloggingApp.Pages.Dashboard;

namespace PloggingApp.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly PlogTogetherViewModel vm;
    public DashboardPage(PlogTogetherViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    private async void TestScan(object sender, EventArgs e)
    {
        await vm.DeleteGroup();
    }
}