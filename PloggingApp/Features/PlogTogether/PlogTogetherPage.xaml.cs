namespace PlogPal.Maui.Features.Plogtogether;

public partial class PlogTogetherPage : ContentPage
{
    public PlogTogetherPage(/*PlogTogetherViewModel vm*/)
    {
        InitializeComponent();
        //BindingContext = vm;
    }

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }
}
