using CommunityToolkit.Maui.Views;
using PloggingApp.MVVM.ViewModels.Popups;

namespace PloggingApp.MVVM.Views;

public partial class EventPopup : Popup
{
	public EventPopup(EventPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}