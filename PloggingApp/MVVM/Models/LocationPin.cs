
using Microsoft.Maui.Controls.Maps;

namespace PloggingApp.MVVM.Models;

public class LocationPin : Pin
{
    public string? Description { get; set; }
    public string? Address { get; set; }
    public Location? Location { get; set; }
    public ImageSource? ImageSource { get; set; }
}
