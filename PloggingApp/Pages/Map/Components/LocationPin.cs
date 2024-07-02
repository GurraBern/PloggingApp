using CommunityToolkit.Mvvm.Input;
using PlogPal.Domain.Models;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace PloggingApp.Features.Map.Components;

public class LocationPin : CustomPin
{
    public string? Description { get; set; }
    public string? Address { get; set; }
    public Location? Location { get; set; }
    public ImageSource? ImageSource { get; set; }
    public IAsyncRelayCommand Command { get; set; }

}

public class LitterbagPlacementPin: LocationPin
{
    public LitterbagPlacement LitterBagPlacement { get; set; }
    public LitterbagPlacementPin(IAsyncRelayCommand command, LitterbagPlacement litterbagPlacement)
    {
        ImageSource = ImageSource.FromFile("handshake_color_icon.png");
        Command = command;
        LitterBagPlacement = litterbagPlacement;
    }
}

public class FinishPin : LocationPin
{
    public FinishPin()
    {
        ImageSource = ImageSource.FromFile("finishpin.png");
    }
}

public class StartPin : LocationPin
{
    public StartPin()
    {
        ImageSource = ImageSource.FromFile("startpin.png");
    }
}

public class CollectedLitterPin : LocationPin
{
    public CollectedLitterPin()
    {
        ImageSource = ImageSource.FromFile("pickeduptrash.png");
    }
}
