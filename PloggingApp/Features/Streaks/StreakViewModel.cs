//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using CommunityToolkit.Mvvm.Messaging;
//using PloggingApp.Data.Services;
//using PloggingApp.Services.Authentication;
//using PloggingApp.Shared;

//namespace PloggingApp.Features.Streak;

//public partial class StreakViewModel : BaseViewModel, IAsyncInitialization, IRecipient<UpdateStreakMessage>
//{
//    private readonly IStreakService _streakService;
//    private readonly IAuthenticationService _authenticationService;

//    public Task Initialization { get; private set; }

//    [ObservableProperty]
//    private int userStreakCount;
//    private string UserId => _authenticationService.UserId;

//    public StreakViewModel(IStreakService streakService, IAuthenticationService authenticationService)
//	{
//        _streakService = streakService;
//        _authenticationService = authenticationService;
//        Initialization = InitializeAsync();

//        WeakReferenceMessenger.Default.Register<UpdateStreakMessage>(this);
//    }

//    private async Task InitializeAsync()
//    {
//        await GetUserStreak();
//    }

//    [RelayCommand]
//    private async Task GetUserStreak()
//    {
//        IsBusy = true;

//        var user = await _streakService.GetUserStreak(UserId);

//        if (user == null)
//        {
//            return;
//        }

//        UserStreakCount = user.Streak;
//        IsBusy = false;
//    }

//    public void Receive(UpdateStreakMessage message)
//    {
//        IsBusy = true;

//        var streak = message.Count;
//        UserStreakCount = streak;

//        IsBusy = false;
//    }
//}

