using CommunityToolkit.Maui.Views;

namespace PloggingApp.Features.LitterPickupRequests;

public partial class LitterbagPlacementPopup : Popup
{
	public LitterbagPlacementPopup(LitterbagPlacementViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void ClosePopup(object sender, EventArgs e)
    {
        Close();
    }
}