using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using PloggingApp.MVVM;
using System.Collections.ObjectModel;

namespace PloggingApp.Pages;

public partial class UserEventsViewModel : IAsyncInitialization
{
    private readonly IUserEventService _userEventService;

    public ObservableCollection<UserEvent> UserEvents { get; set; } = [];

    public Task Initialization { get; private set; }

    public UserEventsViewModel(IUserEventService userEventService)
    {
        _userEventService = userEventService;

        Initialization = Initialize(); 
    }

    private async Task Initialize()
    {
        var events = await _userEventService.GetEvents();
        UserEvents.ClearAndAddRange(events);
    }

    //TODO find a way to remove "nameof(UserEventsPage)" while still being able to navigate to CreateEventPage
    [RelayCommand]
    private async Task CreateEvent()
    {
        await Shell.Current.GoToAsync($"{nameof(UserEventsPage)}/{nameof(CreateEventPage)}");
    }
}
