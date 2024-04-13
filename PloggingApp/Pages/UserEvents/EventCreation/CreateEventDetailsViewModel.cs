using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class CreateEventDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IUserEventService _userEventService;
    [ObservableProperty]
    public MapPoint? location;


    [ObservableProperty]
    private string locationName;

    [ObservableProperty]
    private string eventTitle;

    [ObservableProperty]
    private string eventDescription;

    [ObservableProperty]
    private DateTime eventDate;

    public CreateEventDetailsViewModel(IUserEventService userEventService)
    {
        _userEventService = userEventService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Location = query["Location"] as MapPoint;
        LocationName = "query[LocationName] as string";
    }

    [RelayCommand]
    private async Task CreateEvent()
    {
        //    //TODO validation
        //    if(Location is null)

        var userEvent = new UserEvent()
        {
            Title = EventTitle,
            Description = EventDescription,
            StarDate = EventDate,
            Location = Location
        };

        await _userEventService.CreateEvent(userEvent);
    }
}
