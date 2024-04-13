using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.Pages;

public partial class UserEventsViewModel
{
    public ObservableCollection<UserEvent> UserEvents { get; set; } = [];

    public UserEventsViewModel()
    {
    }

    //TODO find a way to remove "nameof(UserEventsPage)" while still being able to navigate to CreateEventPage
    [RelayCommand]
    private async Task CreateEvent()
    {
        await Shell.Current.GoToAsync($"{nameof(UserEventsPage)}/{nameof(CreateEventPage)}");
    }
}
