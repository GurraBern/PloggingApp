using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Pages;
using PloggingApp.Services.Authentication;
using PloggingApp.MVVM.Models;

namespace PloggingApp.MVVM.ViewModels;

public partial class PlogTogetherViewModel : BaseViewModel, IAsyncInitialization
{
	private readonly IPlogTogetherService _plogTogetherService;
	private readonly IAuthenticationService _authenticationService;
	private readonly IUserInfoService _userInfoService;

    public Task Initialization { get; private set; }

    public ObservableCollection<PlogUser> NewGroup { get; set; } = new ObservableCollection<PlogUser>();

    public List<string> usersInGroupId = new();

    public ICommand RemoveUserCommand { get; private set; }

    public PlogTogetherViewModel(IPlogTogetherService plogTogetherService, IAuthenticationService authenticationService,
								IUserInfoService userInfoService)
	{
		_plogTogetherService = plogTogetherService;
		_authenticationService = authenticationService;
		_userInfoService = userInfoService;
        Initialization = InitializeAsync();
        RemoveUserCommand = new Command<string>(async (userId) => await RemoveUser(userId));
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

        
        await _plogTogetherService.AddUserToGroup(ownerUserId, addUserId);
        usersInGroupId.Add(user.UserId);

        PlogUser plogUser = new()
        {
            DisplayName = user.DisplayName,
            UserId = user.UserId
        };

        
        NewGroup.Add(plogUser);
        
        await Shell.Current.GoToAsync($"//{nameof(PlogTogetherPage)}");
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Application.Current.MainPage.DisplayAlert("Success", $"{user.DisplayName} added to group", "OK");
        });
        

        IsBusy = false;
    }

    [RelayCommand]
	public async Task DeleteGroup()
	{
        IsBusy = true;

        var ownerUserId = _authenticationService.CurrentUser.Uid;
        await _plogTogetherService.DeleteGroup(ownerUserId);
		NewGroup.Clear();
		
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
                usersInGroupId.Add(user.UserId);

                PlogUser plogUser = new()
                {
                    DisplayName = user.DisplayName,
                    UserId = user.UserId
                };

                NewGroup.Add(plogUser);
			}
        }
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			//await Shell.Current.DisplayAlert("Error!", $"Unable to get group: {ex.Message}", "OK");
		}
		finally
		{
            IsBusy = false;
        }
	}

    
    public async Task RemoveUser(string userId)
    {
        IsBusy = true;

        var ownerUserId = _authenticationService.CurrentUser.Uid;
        var user = await _userInfoService.GetUser(userId);

        usersInGroupId.Remove(user.UserId);

        await _plogTogetherService.RemoveUserFromGroup(ownerUserId, userId);

        var plogUser = NewGroup.FirstOrDefault(u => u.UserId == user.UserId);
        NewGroup.Remove(plogUser);

        IsBusy = false;
    }
}

