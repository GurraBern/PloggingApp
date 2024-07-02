using CommunityToolkit.Maui.Views;

namespace PlogPal.Maui.Features.Statistics;

public partial class StatisticsView : ContentView
{
	//private StatisticsViewModel ViewModel { get; set; }

	public StatisticsView()
	{
		InitializeComponent();
	}

	private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		//if (ViewModel is null)
		//{
		//	if (BindingContext != null)
		//		ViewModel = (StatisticsViewModel)BindingContext;
		//	else
		//		return;
		//}
		//RadioButton radioButton = sender as RadioButton;
		//if (radioButton.ContentAsString() == "Year")
		//{
		//	ViewModel.ShowYearCommand.Execute(null);
		//}
		//else
		//{
		//	ViewModel.ShowMonthCommand.Execute(null);
		//}
	}

	private void ShowInformationPopup(object sender, EventArgs e)
	{
		var informationPopup = new StatisticsInformationPopup();
		//Application.Current?.MainPage?.ShowPopup(informationPopup);
	}
}