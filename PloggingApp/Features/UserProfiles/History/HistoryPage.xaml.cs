namespace PloggingApp.Features.UserProfiles;

public partial class HistoryPage : ContentPage
{
	public HistoryPage(HistoryPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}