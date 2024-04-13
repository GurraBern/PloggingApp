using Microcharts;
using PloggingApp.MVVM.ViewModels;
using SkiaSharp;

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
} 