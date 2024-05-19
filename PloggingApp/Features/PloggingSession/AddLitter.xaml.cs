namespace PloggingApp.MVVM.Views;

public partial class AddLitter : ContentView
{
    private bool isExpanded = false;

    public AddLitter()
	{
		InitializeComponent();

        this.TranslateTo(85, 0, 500);
    }

    private void ShowMoreLitterButtons(object sender, EventArgs e)
    {
        if(!isExpanded)
        {
            this.TranslateTo(0, 0, 300, Easing.CubicOut);
            expandBtn.RotateTo(-180, 150);
        }
        else
        {
            this.TranslateTo(85, 0, 300, Easing.CubicOut);
            expandBtn.RotateTo(0, 150);
        }

        isExpanded = !isExpanded;
    }
}