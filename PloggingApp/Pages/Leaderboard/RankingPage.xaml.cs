namespace PloggingApp.Pages;

public partial class RankingPage : ContentPage
{
    public RankingPage(RankingViewmodel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}