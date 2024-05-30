namespace PlogPal.Maui.Features.UserProfiles.Badges;

public partial class BadgesView : ContentView
{
    public BadgesView()
    {
        InitializeComponent();
    }

    private void BadgeTap(object sender, EventArgs e)
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