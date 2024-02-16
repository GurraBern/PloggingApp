using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class MapViewModel : ObservableObject
{
    public ObservableCollection<LocationPin> PlacedPins { get; set; } = [];

    public MapViewModel()
    {

    }

    [RelayCommand]
    public void AddPin()
    {
        var pin = new LocationPin()
        {
            Label = "Test",
            Location = new Location(57.683071, 11.990950),
            Address = "Ukraine",
            ImageSource = ImageSource.FromUri(new Uri("https://www.gamesatlas.com/images/football/teams/ukraine/dynamo-kyiv.png")),
        };

        var pin2 = new LocationPin()
        {
            Label = "Test2",
            Location = new Location(57.682071, 11.990450),
            Address = "Ukraine",
            ImageSource = ImageSource.FromUri(new Uri("https://www.gamesatlas.com/images/football/teams/ukraine/dynamo-kyiv.png")),
        };

        PlacedPins.Add(pin);
        PlacedPins.Add(pin2);
    }
}
