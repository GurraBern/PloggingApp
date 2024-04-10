using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PloggingApp.Data.Services;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Services.Authentication;


namespace PloggingApp.MVVM.ViewModels;

public partial class StreakViewModel : BaseViewModel, IAsyncInitialization, IRecipient<UpdateStreakMessage>
{
    private readonly IStreakService _streakService;
    private readonly IAuthenticationService _authenticationService;

    public Task Initialization { get; private set; }

    [ObservableProperty]
    private int userStreakCount;

    public StreakViewModel(IStreakService streakService, IAuthenticationService authenticationService)
	{
        _streakService = streakService;
        _authenticationService = authenticationService;
        Initialization = InitializeAsync();

        WeakReferenceMessenger.Default.Register<UpdateStreakMessage>(this);
    }

    private async Task InitializeAsync()
    {
        await GetUserStreak();
    }

    [RelayCommand]
    private async Task GetUserStreak()
    {
        var currentUserId = _authenticationService.CurrentUser.Uid; 

        IsBusy = true;

        var user = await _streakService.GetUserStreak(currentUserId);
        UserStreakCount = user.Streak;

        IsBusy = false;
    }

    public void Receive(UpdateStreakMessage message)
    {
        IsBusy = true;

        var streak = message.Count;
        UserStreakCount = streak;

        IsBusy = false;
    }
}

