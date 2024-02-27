using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class MapView : ContentView, IRecipient<PloggingSessionViewModel>
{
    //public MapViewModel MapViewModel { get; set; }

    public MapView()
    {
        //MoveMapToCurrentLocationAsync();
        InitializeComponent();
        //Initialize();
    }

    //private void Initialize()//TODO maybe need to be onappearing
    //{
    //    MapViewModel = (MapViewModel)BindingContext;
    //}

    public void DrawPolyLine(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        Polyline Polyline = ((MapViewModel)BindingContext).Polyline;
        PloggingMap.MapElements.Add(Polyline);
        Polyline.Geopath.Clear();
        //StopSessionButtons();


    }

    public void ResumeClick(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        //InSessionButtons();
        MapFollowFunction();
    }

    public void FinishClick(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        //FinishSessionButtons();
        //MoveMapToCurrentLocationAsync();

    }

    public void StartClick(object sender, EventArgs e)
    {
        PloggingMap.MapElements.Clear();
        //InSessionButtons();
        MapFollowFunction();
    }

    //public async Task<Location> MoveMapToCurrentLocationAsync()
    //{
    //    try
    //    {
    //        var request = new GeolocationRequest(GeolocationAccuracy.Best);
    //        var location = await Geolocation.GetLocationAsync(request);

    //        if (location != null)
    //        {
    //            PloggingMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
    //            return location;
    //        }
    //        else
    //        {
    //            // Handle case where location is null
    //            return null;
    //        }
    //    }
    //    catch (FeatureNotSupportedException fnsEx)
    //    {
    //        // Handle not supported on device exception
    //        return null;
    //    }
    //    catch (PermissionException pEx)
    //    {
    //        // Handle permission exception
    //        return null;
    //    }
    //    catch (Exception ex)
    //    {
    //        // Unable to get location
    //        return null;
    //    }
    //}

    private async void MapFollowFunction()
    {
        //while (StopButton.IsVisible)
        //{
        //await MoveMapToCurrentLocationAsync();
        //}
        Location ZoomLoc = ((MapViewModel)BindingContext).CalculateZoomOut();
        var (Longitude, Latitude) = ((MapViewModel)BindingContext).ZoomRegion();
        MapSpan MapSpan = new MapSpan(ZoomLoc, Longitude * 1.8, Latitude * 1.4);
        PloggingMap.MoveToRegion(MapSpan);
    }

    public void Receive(PloggingSessionViewModel message)
    {
        var t = 5;

    }

    //private void InSessionButtons()
    //{
    //    StartButton.IsVisible = false;
    //    StopButton.IsVisible = true;
    //    ResumeButton.IsVisible = false;
    //    HelpButton.IsVisible = true;
    //    FoundTrashButton.IsVisible = true;
    //    FinishButton.IsVisible = false;
    //    RemoveButton.IsVisible = true;
    //}


    //private void FinishSessionButtons()
    //{
    //    StartButton.IsVisible = true;
    //    StopButton.IsVisible = false;
    //    ResumeButton.IsVisible = false;
    //    FinishButton.IsVisible = false;
    //    HelpButton.IsVisible = false;
    //    FoundTrashButton.IsVisible = false;
    //    RemoveButton.IsVisible = false;
    //}

    //private void StopSessionButtons()
    //{
    //    ResumeButton.IsVisible = true;
    //    FinishButton.IsVisible = true;
    //    StopButton.IsVisible = false;
    //    HelpButton.IsVisible = false;
    //    FoundTrashButton.IsVisible = false;

    //}
}