using PloggingApp.MVVM.ViewModels;
using CommunityToolkit.Maui.Views;
using PloggingApp.MVVM.Views;
namespace PloggingApp.MVVM.Views;

public partial class StatisticsView : ContentView
{
	private StatisticsViewModel viewModel {  get; set; }
	public StatisticsView()
	{
		InitializeComponent();
	}

	private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (viewModel is null)
		{
			if (BindingContext != null)
				viewModel = (StatisticsViewModel)BindingContext;
			else
				return;
		}
		RadioButton radioButton = sender as RadioButton;
		if (radioButton.ContentAsString() == "Year")
		{
			viewModel.ShowYearCommand.Execute(null);
		}
		else
		{
			viewModel.ShowMonthCommand.Execute(null);
		}
	}

	private void ShowInformationPopup(object sender, EventArgs e)
	{
		var informationPopup = new StatisticsInformationPopup();
		Application.Current?.MainPage?.ShowPopup(informationPopup);
	}
} 