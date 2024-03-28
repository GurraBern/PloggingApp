using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Firebase.Auth;
using PloggingApp.MVVM.Models.Messages;
using PloggingApp.Data.Services;
using PloggingApp.Services.Authentication;
using PloggingApp.MVVM.Models;

namespace PloggingApp.Pages;

[QueryProperty(nameof(AddUser), nameof(AddUser))]
public partial class ScanQRcodePageViewModel : ObservableObject
{
    [ObservableProperty]
    private PlogUser addUser;

    private readonly IPlogTogetherService _plogTogetherService;
    private readonly IUserInfoService _userInfoService;
    private readonly IAuthenticationService _authenticationService;

    public ScanQRcodePageViewModel(IPlogTogetherService plogTogetherService, IUserInfoService userInfoService, IAuthenticationService authenticationService)
	{
        _plogTogetherService = plogTogetherService;
        _userInfoService = userInfoService;
        _authenticationService = authenticationService;
	}

	public async Task AddUserToGroup(string userId)
    {
        var ownerUserId = _authenticationService.CurrentUser.Uid;
        var user = await _userInfoService.GetUser(userId);

        await _plogTogetherService.AddUserToGroup(ownerUserId, userId);

        // check if user got added to group, if not then owner or user is part of another group
        // first case: user creates a group and could not add the user
        var plogOwnerGroup = await _plogTogetherService.GetPlogTogether(ownerUserId);
        if (plogOwnerGroup == null)
        {
            await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"You or {user.DisplayName} exists in another group!", "OK");
            });
            
        }
        // second case: user owns a group and could not add the user
        else if (!plogOwnerGroup.UserIds.Contains(user.UserId))
        {
            await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"{user.DisplayName} exists in another group!", "OK");
            });
        }
        else
        {
            // user has been added to this group, send update to view
            PlogUser plogUser = new()
            {
                DisplayName = user.DisplayName,
                UserId = user.UserId
            };

            AddUser = plogUser;

            WeakReferenceMessenger.Default.Send(new AddUserMessage(AddUser));

            await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
        }
    }
}
