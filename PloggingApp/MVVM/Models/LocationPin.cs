using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Plogging.Core.Models;
using PloggingApp.MVVM.Views.Components;
using System.Windows.Input;

namespace PloggingApp.MVVM.Models;

public class LocationPin : CustomPin
{
    public string? Description { get; set; }
    public string? Address { get; set; }
    public Location? Location { get; set; }
    public ImageSource? ImageSource { get; set; }
    public ICommand Command { get; set; }
}

public class NeedHelpToCollectPin : LocationPin
{
    public NeedHelpToCollectPin()
    {
        ImageSource = ImageSource.FromFile("needhelptrashpin.png");
    }

}

public class TrashCollectedPin : LocationPin
{
    public TrashCollectedPin()
    {

    }

}

public class LitterBagPlacementPin: LocationPin
{
    public LitterBagPlacement LitterBagPlacement { get; set; }
    public LitterBagPlacementPin(ICommand command, LitterBagPlacement litterBagPlacement)
    {
        ImageSource = ImageSource.FromFile("handshake_color_icon.png");
        Command = command;
        LitterBagPlacement = litterBagPlacement;
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

public class PlasticPin : LocationPin
{
    public PlasticPin()
    {
    }

}

public class CanPin : LocationPin
{
    public CanPin()
    {
        ImageSource = ImageSource.FromFile("canpin.png");

    }

}
public class CigarettePin : LocationPin
{
    public CigarettePin()
    {

    }

}

public class SnusPin : LocationPin
{
    public SnusPin()
    {

    }

}