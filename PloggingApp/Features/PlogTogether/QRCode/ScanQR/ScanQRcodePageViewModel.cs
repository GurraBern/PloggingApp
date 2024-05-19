using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Data.Services;
using PloggingApp.Services.Authentication;
using PloggingApp.MVVM.Models;
using PloggingApp.Shared;

namespace PloggingApp.Pages;

[QueryProperty(nameof(AddUser), nameof(AddUser))]
public partial class ScanQRcodePageViewModel : ObservableObject
{
    private readonly IPlogTogetherService _plogTogetherService;
    private readonly IUserInfoService _userInfoService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IToastService _toastService;

    [ObservableProperty]
    private PlogUser addUser;

    [ObservableProperty]
    private string userId;

    public ScanQRcodePageViewModel(IPlogTogetherService plogTogetherService, IUserInfoService userInfoService,
                                    IAuthenticationService authenticationService, IToastService toastService)
	{
        _plogTogetherService = plogTogetherService;
        _userInfoService = userInfoService;
        _authenticationService = authenticationService;
        _toastService = toastService;
	}

    public async Task AddUserToGroup(string userId)
    {
        var currentUserId = _authenticationService.UserId;
        var userInfo = await _userInfoService.GetUser(userId);

        var plogTogetherGroup = await _plogTogetherService.GetPlogTogether(currentUserId);

        if (plogTogetherGroup == null)
        {
            var userAlreadyInGroup = await _plogTogetherService.GetPlogTogether(userId);
            if (userAlreadyInGroup == null)
            {
                if (currentUserId == userId)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
                        await _toastService.MakeToast("Can't add yourself!");
                    });
                    return;
                }
                await _plogTogetherService.AddUserToGroup(currentUserId, userId);

                var ownerUserInfo = await _userInfoService.GetUser(currentUserId);

                PlogUser plogUser1 = new()
                {
                    DisplayName = ownerUserInfo.DisplayName
                };

                PlogUser plogUser2 = new()
                {
                    DisplayName = userInfo.DisplayName
                };

                List<PlogUser> plogUsers = new()
                {
                    plogUser1,
                    plogUser2
                };

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
                    await _toastService.MakeToast("User added to group!");
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
                    await _toastService.MakeToast("Error, user is already in a group!");
                });
            }
        }
        else
        {
            if (plogTogetherGroup.OwnerUserId == currentUserId)
            {
                var userAlreadyInGroup = await _plogTogetherService.GetPlogTogether(userId);
                if (userAlreadyInGroup == null)
                {
                    await _plogTogetherService.AddUserToGroup(currentUserId, userId);

                    PlogUser plogUser = new()
                    {
                        DisplayName = userInfo.DisplayName
                    };

                    //WeakReferenceMessenger.Default.Send(new AddUserMessage(plogUser));

                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
                        await _toastService.MakeToast("User added to group!");
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
                        await _toastService.MakeToast("Error, user is already in a group!");
                    });
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
                    await _toastService.MakeToast("Error, can't add user: you are not the owner of the group!");
                });
            }
        }
    }
}
