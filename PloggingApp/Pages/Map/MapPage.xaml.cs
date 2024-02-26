namespace PloggingApp.Pages;

public partial class MapPage : ContentPage
{
    public MapPage(MapPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}