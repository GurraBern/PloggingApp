namespace PloggingApp.Pages;

public partial class OthersProfilePage : ContentPage
{
    private readonly OthersProfilePageViewModel vm;

    public OthersProfilePage(OthersProfilePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await vm.OthersSessionsViewModel.UpdatePage();
        await vm.BadgesViewModel.Init();
    }
}
