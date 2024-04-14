using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class UserEventsPage : ContentPage
{
	public UserEventsPage(UserEventsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}