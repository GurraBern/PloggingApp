﻿using CommunityToolkit.Mvvm.Input;
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

public class LitterbagPlacementPin: LocationPin
{
    public LitterbagPlacement LitterBagPlacement { get; set; }
    public LitterbagPlacementPin(ICommand command, LitterbagPlacement litterbagPlacement)
    {
        ImageSource = ImageSource.FromFile("handshake_color_icon.png");
        Command = command;
        LitterBagPlacement = litterbagPlacement;
    }
}

public class UserEventPin : LocationPin
{
    public UserEvent UserEvent { get; set; }
    public UserEventPin(ICommand command, UserEvent userEvent)
    {
        ImageSource = ImageSource.FromFile("event_calendar_icon.png");
        Command = command;
        UserEvent = userEvent;
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
        ImageSource = ImageSource.FromFile("red_dot.png");
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