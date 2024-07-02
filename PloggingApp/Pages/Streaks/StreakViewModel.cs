using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PloggingApp.Features.Streak;
using PlogPal.Application.Common.Interfaces;
using PlogPal.Maui.Shared;

namespace PlogPal.Maui.Features.Streak;

public partial class StreakViewModel : BaseViewModel, IAsyncInitialization, IRecipient<UpdateStreakMessage>
{
    private readonly IStreakManager _streakManager;
    private readonly IUserContext _userContext;
    public Task Initialization { get; }

    [ObservableProperty]
    private int userStreakCount;

    public StreakViewModel(IStreakManager streakManager, IUserContext userContext)
	{
        _streakManager = streakManager;
        _userContext = userContext;

        UserStreakCount = _userContext.User.Streak;
        
        // WeakReferenceMessenger.Default.Register(this);
    }

    // private async Task InitializeAsync()
    // {
    //     await GetUserStreak();
    // }
    //
    // [RelayCommand]
    // private void GetUserStreak()
    // {
    //     UserStreakCount = _userContext.User.Streak;
    // }

    //TODO instead of message, use update method that triggers on appearing?
    public void Receive(UpdateStreakMessage message)
    {
        IsBusy = true;

        var streak = message.Count;
        UserStreakCount = streak;

        IsBusy = false;
    }
}

