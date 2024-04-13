namespace PloggingApp.Pages;

public partial class HistoryPage : ContentPage
{
	public HistoryPage(HistoryPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}