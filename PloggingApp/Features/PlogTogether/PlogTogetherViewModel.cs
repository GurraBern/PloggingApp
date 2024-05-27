using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PloggingApp.Shared;
using PloggingApp.Features.Dashboard;
using PloggingApp.Features.PloggingSession;

namespace PloggingApp.Features.Plogtogether;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class PlogTogetherViewModel : BaseViewModel, IAsyncInitialization, IRecipient<PloggingSessionMessage>
{
	private readonly IPlogTogetherService _plogTogetherService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserInfoService _userInfoService;
    private readonly IToastService _toastService;

    public Task Initialization { get; private set; }

    public ObservableCollection<PlogUser> Group { get; set; } = [];

    [ObservableProperty]
    private bool isTracking = false;

    [ObservableProperty]
    private bool inGroup = false;

    [ObservableProperty]
    private bool userCanAdd = true;

    [ObservableProperty]
    private bool userCanLeave = false;

    [ObservableProperty]
    private bool userCanDelete = false;

    //private string UserId => _authenticationService.UserId;

    [ObservableProperty]
    private string userId;

    public ICommand RemoveUserCommand { get; private set; }

    public PlogTogetherViewModel(IPlogTogetherService plogTogetherService, IAuthenticationService authenticationService,
								IUserInfoService userInfoService, IToastService toastService)
	{
		_plogTogetherService = plogTogetherService;
        _authenticationService = authenticationService;
        _userInfoService = userInfoService;
        _toastService = toastService;

        Initialization = InitializeAsync();
        RemoveUserCommand = new Command<string>(async (userId) => await RemoveUser(userId));

        WeakReferenceMessenger.Default.Register<PloggingSessionMessage>(this);
    }

    private async Task InitializeAsync()
    {
        await GetGroup();
    }

    public void Receive(PloggingSessionMessage message)
    {
        IsTracking = message.IsTracking;
    }

    [RelayCommand]
	public async Task DeleteGroup()
	{
        IsBusy = true;

        var plogGroup = await _plogTogetherService.GetPlogTogether(UserId);

        if (plogGroup == null)
        {
            ClearGroup();
            IsBusy = false;
            return;
        }

        if (IsTracking == false)
        {
            if (plogGroup.OwnerUserId == UserId)
            {
                await _plogTogetherService.DeleteGroup(UserId);
                ClearGroup();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await _toastService.MakeToast("Can't delete group: only the owner can delete the group!");
                });
            }
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _toastService.MakeToast("Can't delete group: active plogging session!");
            });
        }
		
		IsBusy = false;
    }

	[RelayCommand]
	public async Task GetGroup()
	{
        IsBusy = true;

        var plogTogether = await _plogTogetherService.GetPlogTogether(UserId);

       
        if (plogTogether == null)
        {
            ClearGroup();
            IsBusy = false;
            return;
        }

        InGroup = true;

        if (UserId == plogTogether.OwnerUserId)
        {
            foreach (var memberId in plogTogether.UserIds)
            {
                var user = await _userInfoService.GetUser(memberId);
                if (user != null)
                {
                    if (memberId == plogTogether.OwnerUserId)
                    {
                        Group.Add(new PlogUser
                        {
                            DisplayName = user.DisplayName + " (leader)",
                            UserId = user.UserId,
                            ShowButtons = false
                        });
                    }
                    else
                    {
                        Group.Add(new PlogUser
                        {
                            DisplayName = user.DisplayName,
                            UserId = user.UserId,
                            ShowButtons = true
                        });
                    }
                }
            }

            UserCanAdd = true;
            UserCanDelete = true;
        }
        else
        {
            foreach (var memberId in plogTogether.UserIds)
            {
                var user = await _userInfoService.GetUser(memberId);
                if (user != null)
                {
                    if (memberId == plogTogether.OwnerUserId)
                    {
                        Group.Add(new PlogUser
                        {
                            DisplayName = user.DisplayName + " (leader)",
                            UserId = user.UserId,
                            ShowButtons = false
                        });
                    }
                    else
                    {
                        Group.Add(new PlogUser
                        {
                            DisplayName = user.DisplayName,
                            UserId = user.UserId,
                            ShowButtons = false
                        });
                    }
                }
            }
            UserCanAdd = false;
            UserCanLeave = true;
            UserCanDelete = false;
        }

        IsBusy = false;
	}

    // ska man kunna lämna under active session? bara att kolla isTracking isf
    [RelayCommand]
    public async Task LeaveGroup()
    {
        IsBusy = true;

        var plogGroup = await _plogTogetherService.GetPlogTogether(UserId);

        if (plogGroup == null)
        {
            ClearGroup();
            IsBusy = false;
            return;
        }
        
        if (plogGroup.OwnerUserId != UserId)
        {
            await _plogTogetherService.LeaveGroup(UserId);
            ClearGroup();
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _toastService.MakeToast("Can't leave group: you are the leader!");
            });
        }
        
        IsBusy = false;
    }

    public async Task RemoveUser(string userId)
    {
        IsBusy = true;

        var plogUser = Group.FirstOrDefault(u => u.UserId == userId);

        if (plogUser != null)
        {
            await _plogTogetherService.LeaveGroup(userId);
            Group.Remove(plogUser);

            var plogGroup = await _plogTogetherService.GetPlogTogether(UserId);
            if (plogGroup == null)
            {
                ClearGroup();
            }
        }

        IsBusy = false;
    }

    [RelayCommand]
    public async Task GoBack()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        IsBusy = false;
    }

    [RelayCommand]
    private async Task GoToGenerateQRcode()
    {
        await Shell.Current.GoToAsync($"{nameof(GenerateQRcodePage)}?UserId={UserId}");
    }

    [RelayCommand]
    private async Task GoToScanQRcode()
    {
        await Shell.Current.GoToAsync(nameof(ScanQRcodePage));
    }

    private void ClearGroup()
    {
        InGroup = false;
        UserCanAdd = true;
        UserCanLeave = false;
        UserCanDelete = false;
        Group.Clear();
    }
}

