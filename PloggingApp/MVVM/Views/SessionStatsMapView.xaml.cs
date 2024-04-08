using PloggingApp.MVVM.ViewModels;
using Microsoft.Maui.Controls.Maps;
using PloggingApp.MVVM.Models;

namespace PloggingApp.MVVM.Views;

public partial class SessionStatsMapView : ContentView
{
	public SessionStatsMapViewModel ViewModel { get; set; }
	public SessionStatsMapView()
	{
		InitializeComponent();
		Initialize();
	}

	private void Initialize()
	{
		ViewModel = (SessionStatsMapViewModel)BindingContext;
		DrawRoutePolyLine();
	}

	private void DrawRoutePolyLine()
	{
		if (ViewModel == null)
			return;
		Map.MapElements.Clear(); //??
		polyLine = new Polyline()
		{
			StrokeColor = Colors.Blue,
			StrokeWidth = 10
		};
		List<Location> locations = ViewModel.LocationPins.Select(x => new Location(x.Latitude, x.Longitude)).ToList();
		foreach(var location in locations)
		{
			polyLine.Geopath.Add(location);
		}
		Map.MapElements.Add(polyLine);
	}

    private void CustomPin_MarkerClicked(object sender, PinClickedEventArgs e)
    {
		return;
    }
}