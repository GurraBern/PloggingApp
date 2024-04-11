using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;


using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.Models.Messages;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Pages;
using PloggingApp.Services.Authentication;
using System.Diagnostics;
using PloggingApp.Services.PloggingTracking;
using Firebase.Auth;
using PloggingApp.Services;

namespace PloggingApp.MVVM.ViewModels;

public partial class PlogTogetherViewModel : BaseViewModel, IAsyncInitialization,
                                             IRecipient<PloggingSessionMessage>
{
	private readonly IPlogTogetherService _plogTogetherService;
	private readonly IAuthenticationService _authenticationService;
	private readonly IUserInfoService _userInfoService;
    private readonly IToastService _toastService;

    public Task Initialization { get; private set; }

    public ObservableCollection<PlogUser> Group { get; set; } = new ObservableCollection<PlogUser>();

    private bool isTracking = false;

    [ObservableProperty]
    private bool inGroup = false;

    public ICommand RemoveUserCommand { get; private set; }

    public PlogTogetherViewModel(IPlogTogetherService plogTogetherService, IAuthenticationService authenticationService,
								IUserInfoService userInfoService, IToastService toastService)
	{
		_plogTogetherService = plogTogetherService;
		_authenticationService = authenticationService;
		_userInfoService = userInfoService;
        _toastService = toastService;

        Initialization = InitializeAsync();
        //RemoveUserCommand = new Command<string>(async (userId) => await RemoveUser(userId));

        WeakReferenceMessenger.Default.Register<PloggingSessionMessage>(this);
    }

    private async Task InitializeAsync()
    {
        await GetGroup();
    }

    public void Receive(PloggingSessionMessage message)
    {
        isTracking = message.IsTracking;
    }

    [RelayCommand]
	public async Task DeleteGroup()
	{
        IsBusy = true;

        if (isTracking == false)
        {
            var currentUserId = _authenticationService.CurrentUser.Uid;
            var plogGroup = await _plogTogetherService.GetPlogTogether(currentUserId);

            if (plogGroup == null) return;

            if (plogGroup.OwnerUserId == currentUserId)
            {
                await _plogTogetherService.DeleteGroup(currentUserId);
                Group.Clear();
                InGroup = false;
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

        var userId = _authenticationService.CurrentUser.Uid;
        var plogTogether = await _plogTogetherService.GetPlogTogether(userId);

		if (plogTogether == null)
        {
            InGroup = false;
            return;
        }

        InGroup = true;

		foreach (var memberId in plogTogether.UserIds)
		{
			var user = await _userInfoService.GetUser(memberId);
            if (user != null)
            {
                Group.Add(new PlogUser
                {
                    DisplayName = user.DisplayName
                });
            }
        }
        IsBusy = false;
	}

    // ska man kunna lämna under active session? bara att kolla isTracking isf
    [RelayCommand]
    public async Task LeaveGroup()
    {
        IsBusy = true;

        var currentUserId = _authenticationService.CurrentUser.Uid;
        var plogGroup = await _plogTogetherService.GetPlogTogether(currentUserId);

        if (plogGroup == null) return;
        
        if (plogGroup.OwnerUserId != currentUserId)
        {
            await _plogTogetherService.LeaveGroup(currentUserId);
            Group.Clear();
            InGroup = false;
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

    // not in use currently. thought to be for the leader to remove a user
    /*public async Task RemoveUser(string userId)
    {
        IsBusy = true;

        var currentUserId = _authenticationService.CurrentUser.Uid;
        var user = await _userInfoService.GetUser(userId);

        await _plogTogetherService.RemoveUserFromGroup(currentUserId, userId);

        var plogUser = NewGroup.FirstOrDefault(u => u.UserId == user.UserId);
        NewGroup.Remove(plogUser);

        IsBusy = false;
    }*/
}

