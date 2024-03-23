using CommunityToolkit.Maui.Views;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class LitterBagPlacementPopup : Popup
{
	public LitterBagPlacementPopup(LitterBagPlacementViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}