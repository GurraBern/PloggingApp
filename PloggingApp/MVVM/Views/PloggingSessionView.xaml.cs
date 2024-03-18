namespace PloggingApp.MVVM.Views;

public partial class PloggingSessionView : ContentView
{
    private bool isOpened = false;

	public PloggingSessionView()
	{
		InitializeComponent();
	}

    private async void OpenMenuActions(object sender, EventArgs e)
    {
        isOpened = !isOpened;

        if (isOpened)
        {
            await menuBtn.FadeTo(0.8, 100, Easing.SinIn);
            menuActions.IsVisible = true;
            await menuActions.FadeTo(1, 100, Easing.SinIn);
        }
        else
        {
            await menuBtn.FadeTo(1, 100, Easing.SinIn);
            await menuActions.FadeTo(0, 100, Easing.SinIn);
            menuActions.IsVisible = false;
        }
    }
}