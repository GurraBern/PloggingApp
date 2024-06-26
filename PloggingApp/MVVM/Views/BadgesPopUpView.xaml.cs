using CommunityToolkit.Maui.Views;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class BadgesPopUpView : Popup
{
	public BadgesPopUpView(BadgesPopUpViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
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