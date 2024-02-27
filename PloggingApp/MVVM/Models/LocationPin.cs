
using Microsoft.Maui.Controls.Maps;

namespace PloggingApp.MVVM.Models;

public class LocationPin : Pin
{
    public string? Description { get; set; }
    public string? Address { get; set; }
    public Location? Location { get; set; }
    public ImageSource? ImageSource { get; set; }
}

public class NeedHelpToCollectPin : LocationPin
{
    public NeedHelpToCollectPin() {
        ImageSource = ImageSource.FromFile("needhelptrashpin.png");
    }

}

public class TrashCollectedPin : LocationPin
{
    public TrashCollectedPin()
    {

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
        ImageSource = ImageSource.FromFile("canpin");

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


