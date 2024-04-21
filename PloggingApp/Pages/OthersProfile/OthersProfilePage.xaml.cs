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

    private void BackTap(object sender, EventArgs e)
    {
        if (sender is Image tappedImage)
        {
            tappedImage.Opacity = 0.5;

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                tappedImage.Opacity = 1;
                return false;
            });
        }
    }

}
