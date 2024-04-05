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
}