using CommunityToolkit.Maui.Views;

namespace PlogPal.Maui.Features.LitterPickupRequests;

public partial class LitterbagPlacementPopup : Popup
{
	public LitterbagPlacementPopup(/*LitterbagPlacementViewModel vm*/)
	{
		//InitializeComponent();
		//BindingContext = vm;
	}

	private void ClosePopup(object sender, EventArgs e)
	{
		Close();
	}
}