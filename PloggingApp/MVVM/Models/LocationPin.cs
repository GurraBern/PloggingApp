﻿using Plogging.Core.Models;
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

public class CanPin : LocationPin
{
    public CanPin()
    {
        ImageSource = ImageSource.FromFile("pickeduptrash.png");
    }
}
