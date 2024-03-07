using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;

namespace PloggingApp.MVVM.ViewModels;

public partial class PlogTogetherViewModel : BaseViewModel
{
	private readonly IPlogTogetherService _plogTogetherService;

    public PlogTogetherViewModel(IPlogTogetherService plogTogetherService)
	{
		_plogTogetherService = plogTogetherService;
	}

	[RelayCommand]
	public async Task AddUserToGroup(string addUserId)
	{
        IsBusy = true;

        //TODO replace with actual id when user authentication is implemented
        var ownerUserId = "333ajsldkfjasödjfk34";

		await _plogTogetherService.AddUserToGroup(ownerUserId, addUserId);
		IsBusy = false;
	}

	[RelayCommand]
	public async Task DeleteGroup()
	{
        IsBusy = true;

        //TODO replace with actual id when user authentication is implemented
        var ownerUserId = "333ajsldkfjasödjfk34";

		await _plogTogetherService.DeleteGroup(ownerUserId);

		IsBusy = false;
    }
}

