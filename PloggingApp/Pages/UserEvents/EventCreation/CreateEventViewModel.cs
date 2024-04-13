using Plogging.Core.Models;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class CreateEventViewModel : BaseViewModel
{
    public async Task SetEventLocation(MapPoint location, string locationName)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Location", location },
            { "LocationName", locationName }
        };

        await Shell.Current.GoToAsync(nameof(CreateEventDetails), navigationParameter);
    }
}