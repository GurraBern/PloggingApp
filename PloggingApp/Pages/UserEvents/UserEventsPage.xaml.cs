using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class UserEventsPage : ContentPage
{
	public UserEventsPage()
	{
		InitializeComponent();

		BindingContext = new UserEventsViewModel();
	}
}