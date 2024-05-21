namespace PloggingApp.Features.UserProfiles;

public partial class OthersProfilePage : ContentPage
{
    private readonly OthersProfilePageViewModel vm;

    public OthersProfilePage(OthersProfilePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        vm.InitializeComponents();
    }

    //TODO change from obsolete
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
