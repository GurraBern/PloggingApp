namespace PloggingApp.Pages;

public partial class SessionStatisticsPage : ContentPage
{
	public SessionStatisticsPage(SessionStatisticsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}