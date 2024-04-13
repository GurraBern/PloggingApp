using Microsoft.Maui.Controls.Maps;
using Plogging.Core.Models;

namespace PloggingApp.Pages;

public partial class CreateEventPage : ContentPage
{
    private readonly CreateEventViewModel vm;

    public CreateEventPage(CreateEventViewModel vm)
	{
		InitializeComponent();
        this.vm = vm;
    }

    private async void MapPlaceEvent(object sender, MapClickedEventArgs e)
    {
        var placemarks = await Geocoding.Default.GetPlacemarksAsync(e.Location.Latitude, e.Location.Longitude);
        var placemark = placemarks?.FirstOrDefault();

        var location = new MapPoint(e.Location.Latitude, e.Location.Longitude);
		await vm.SetEventLocation(location, placemark.AdminArea);

    }
}