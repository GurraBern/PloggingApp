using PloggingApp.MVVM.ViewModels;
using Microsoft.Maui.Controls.Maps;
using PloggingApp.MVVM.Models;
using Microsoft.Maui.Maps;
using CommunityToolkit.Mvvm.Messaging;
using PloggingApp.MVVM.Models.Messages;

namespace PloggingApp.MVVM.Views;

public partial class SessionStatsMapView : ContentView, IRecipient<PloggingSessionMessage>
{
	public SessionStatsMapViewModel ViewModel { get; set; }
	public SessionStatsMapView()
	{
		InitializeComponent();
		WeakReferenceMessenger.Default.Register(this);
	}

	private void Initialize()
	{
		ViewModel = (SessionStatsMapViewModel)BindingContext;
		DrawRoutePolyLine();
		CenterMap();
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

	private async Task CenterMap()
	{
		Location location;
		if (!ViewModel.TrashPins.Any())
		{
			location = await Geolocation.GetLocationAsync();

		}
		else
		{
			location = GetCenterLocation(ViewModel.TrashPins.Select(x => new Location(x.Location.Latitude, x.Location.Longitude)));
		}
		Map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
	}

	private Location GetCenterLocation(IEnumerable<Location> locations)
	{
		var avLatitude = locations.Average(x => x.Latitude);
		var avLongitude = locations.Average(x => x.Longitude);
		return new Location(avLatitude, avLongitude);	
	}
    private void CustomPin_MarkerClicked(object sender, PinClickedEventArgs e)
    {
		return;
    }

    public void Receive(PloggingSessionMessage message)
    {
		if(ViewModel == null)
			Initialize();
    }
}