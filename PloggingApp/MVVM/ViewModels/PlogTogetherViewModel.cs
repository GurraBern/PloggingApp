using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Pages;
using PloggingApp.Services.Authentication;

namespace PloggingApp.MVVM.ViewModels;

public partial class PlogTogetherViewModel : BaseViewModel, IAsyncInitialization
{
	private readonly IPlogTogetherService _plogTogetherService;
	private readonly IAuthenticationService _authenticationService;
	private readonly IUserInfoService _userInfoService;

    public Task Initialization { get; private set; }

    public ObservableCollection<UserInfo> Group { get; set; } = new ObservableCollection<UserInfo>();
    public List<string> userIds = new();

    public PlogTogetherViewModel(IPlogTogetherService plogTogetherService, IAuthenticationService authenticationService,
								IUserInfoService userInfoService)
	{
		_plogTogetherService = plogTogetherService;
		_authenticationService = authenticationService;
		_userInfoService = userInfoService;
        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await GetGroup();
    }

	[RelayCommand]
    public async Task AddUserToGroup(string addUserId)
    {
        IsBusy = true;

        var ownerUserId = _authenticationService.CurrentUser.Uid;
        var user = await _userInfoService.GetUser(addUserId);

        if (userIds.Contains(user.UserId))
        {
            await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User already in group", "OK");
            });
        }
        else
        {
            await _plogTogetherService.AddUserToGroup(ownerUserId, addUserId);
            userIds.Add(user.UserId);
            Group.Add(user);
            Debug.WriteLine(Group);

            await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"{user.DisplayName} added to group", "OK");
            });
        }

        IsBusy = false;
    }

    [RelayCommand]
	public async Task DeleteGroup()
	{
        IsBusy = true;

        var ownerUserId = _authenticationService.CurrentUser.Uid;
        await _plogTogetherService.DeleteGroup(ownerUserId);
		Group.Clear();
		
		IsBusy = false;
    }

	[RelayCommand]
	public async Task GetGroup()
	{
		try
		{
            IsBusy = true;

            var ownerUserId = _authenticationService.CurrentUser.Uid;
            var plogTogether = await _plogTogetherService.GetPlogTogether(ownerUserId);

			if (plogTogether == null)
			{
				return;
			}

            List<string> members = plogTogether.UserIds;

			foreach (var member in members)
			{
				var user = await _userInfoService.GetUser(member);
                userIds.Add(user.UserId);
                Group.Add(user);
			}
        }
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			await Shell.Current.DisplayAlert("Error!", $"Unable to get group: {ex.Message}", "OK");
		}
		finally
		{
            IsBusy = false;
        }
	}
}

