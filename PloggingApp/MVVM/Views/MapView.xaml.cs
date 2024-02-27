﻿using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class MapView : ContentView, IRecipient<PloggingSessionMessage>
{
    public MapViewModel? MapViewModel { get; set; }

    public MapView()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register(this);
    }

    private void Initialize()   
    {
        MapViewModel = (MapViewModel)BindingContext;
    }

    public void DrawPolyLine()
    {
        PloggingMap.MapElements.Clear();

        foreach (var location in MapViewModel.TrackingPositions)
        {
            polyLine.Add(location);
        }
        //Polyline Polyline = ((MapViewModel)BindingContext).Polyline;
        PloggingMap.MapElements.Add(polyLine);
        //Polyline.Geopath.Clear();
    }

    private async Task<Location> MoveMapToCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                PloggingMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
                return location;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private async Task MapFollowFunction()
    {
        await MoveMapToCurrentLocationAsync();

        Location ZoomLoc = ((MapViewModel)BindingContext).CalculateZoomOut();
        var (Longitude, Latitude) = ((MapViewModel)BindingContext).ZoomRegion();
        MapSpan MapSpan = new MapSpan(ZoomLoc, Longitude * 1.8, Latitude * 1.4);
        PloggingMap.MoveToRegion(MapSpan);
    }

    //Bad solution should use data binding if possible
    public void Receive(PloggingSessionMessage message)
    {
        if(MapViewModel == null)
            Initialize();

        if (message.IsTracking)
        {
            MapFollowFunction();
        }
        else
        {
            DrawPolyLine();
        }
    }
}

public class PloggingSessionMessage : ValueChangedMessage<bool>
{
    public PloggingSessionMessage(bool isTracking) : base(isTracking)
    {
        IsTracking = isTracking;
    }

    public bool IsTracking { get; set; }
}