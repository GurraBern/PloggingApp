using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Services.Authentication;

namespace PloggingApp.MVVM.ViewModels;

public partial class PlogTogetherViewModel : BaseViewModel
{
	private readonly IPlogTogetherService _plogTogetherService;
	private readonly IAuthenticationService _authenticationService;

    public PlogTogetherViewModel(IPlogTogetherService plogTogetherService, IAuthenticationService authenticationService)
	{
		_plogTogetherService = plogTogetherService;
		_authenticationService = authenticationService;
	}

	[RelayCommand]
	public async Task AddUserToGroup(string addUserId)
	{
        IsBusy = true;

		var ownerUserId = _authenticationService.CurrentUser.Uid;

		await _plogTogetherService.AddUserToGroup(ownerUserId, addUserId);
		IsBusy = false;
	}

	[RelayCommand]
	public async Task DeleteGroup()
	{
        IsBusy = true;

        var ownerUserId = _authenticationService.CurrentUser.Uid;

        await _plogTogetherService.DeleteGroup(ownerUserId);

		IsBusy = false;
    }
}

