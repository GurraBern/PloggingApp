namespace PloggingApp.Features.Statistics;

public partial class SessionStatisticsPage : ContentPage
{
	public SessionStatisticsPage(SessionStatisticsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}