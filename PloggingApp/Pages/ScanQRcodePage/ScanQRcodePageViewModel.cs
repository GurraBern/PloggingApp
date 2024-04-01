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
        var currentUserId = _authenticationService.CurrentUser.Uid;
        var userInfo = await _userInfoService.GetUser(userId);

        var plogTogetherGroup = await _plogTogetherService.GetPlogTogether(currentUserId);

        if (plogTogetherGroup == null)
        {
            var userAlreadyInGroup = await _plogTogetherService.GetPlogTogether(userId);
            if (userAlreadyInGroup == null)
            {
                await _plogTogetherService.AddUserToGroup(currentUserId, userId);

                var ownerUserInfo = await _userInfoService.GetUser(currentUserId);

                PlogUser plogUser1 = new()
                {
                    DisplayName = ownerUserInfo.DisplayName,
                    UserId = ownerUserInfo.UserId
                };

                PlogUser plogUser2 = new()
                {
                    DisplayName = userInfo.DisplayName,
                    UserId = userInfo.UserId
                };

                List<PlogUser> plogUsers = new()
                {
                    plogUser1,
                    plogUser2
                };

                WeakReferenceMessenger.Default.Send(new AddFirstUserMessage(plogUsers));

                await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Success", $"{userInfo.DisplayName} added to group!", "OK");
                });
            }
            else // user already in a group
            {
                await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Can't add user: user is already in a group", "OK");
                });
            }
        }
        else // currentUser already in a group, check if current user is owner, then check if user trying to add is in group already
        {
            if (plogTogetherGroup.OwnerUserId == currentUserId)
            {
                var userAlreadyInGroup = await _plogTogetherService.GetPlogTogether(userId);
                if (userAlreadyInGroup == null)
                {
                    await _plogTogetherService.AddUserToGroup(currentUserId, userId);

                    PlogUser plogUser = new()
                    {
                        DisplayName = userInfo.DisplayName,
                        UserId = userInfo.UserId
                    };

                    AddUser = plogUser;

                    WeakReferenceMessenger.Default.Send(new AddUserMessage(AddUser));

                    await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", $"{userInfo.DisplayName} added to group!", "OK");
                    });
                }
                else // user already in a group
                {
                    await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Can't add user: user is already in a group", "OK");
                    });
                }
            }
            else //current user is not owner!!! 
            {
                await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Can't add user: you are not the owner of the group", "OK");
                });
            }
        }
    }
}
