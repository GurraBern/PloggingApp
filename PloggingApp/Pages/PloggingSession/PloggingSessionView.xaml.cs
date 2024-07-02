namespace PlogPal.Maui.Features.PloggingSession;

public partial class PloggingSessionView : ContentView
{
    private bool isOpened = false;

    public PloggingSessionView()
    {
        InitializeComponent();

        menuActions.IsEnabled = false;
    }

    private void OpenMenuActions(object sender, EventArgs e)
    {
        isOpened = !isOpened;

        if (isOpened)
        {
            menuActions.IsEnabled = true;

            menuBtn.RotateTo(180, 200, Easing.Default);

            menuBtn.FadeTo(0.8, 100, Easing.SinIn);
            menuActions.IsVisible = true;
            menuActions.FadeTo(1, 150, Easing.SinIn);
        }
        else
        {
            menuBtn.RotateTo(360, 200, Easing.Default);

            menuBtn.FadeTo(1, 100, Easing.SinIn);
            menuActions.FadeTo(0, 150, Easing.SinIn);
            menuActions.IsVisible = false;
            menuActions.IsEnabled = false;
        }
    }
}