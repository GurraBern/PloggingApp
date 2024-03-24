using CommunityToolkit.Maui.Views;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class LitterbagPlacementPopup : Popup
{
	public LitterbagPlacementPopup(LitterbagPlacementViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}