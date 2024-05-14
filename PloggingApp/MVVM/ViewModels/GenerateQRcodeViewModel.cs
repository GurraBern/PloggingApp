using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Services.Authentication;

namespace PloggingApp.MVVM.ViewModels;

public partial class GenerateQRcodeViewModel : BaseViewModel
{
    public Task Initialization { get; private set; }
    private readonly IAuthenticationService _authenticationService;

    public GenerateQRcodeViewModel(IAuthenticationService authenticationService)
	{
        _authenticationService = authenticationService;
        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await GetUserId();
    }

    [ObservableProperty]
	private string currentUserId;

	[RelayCommand]
	private Task GetUserId()
    {
        IsBusy = true;

        CurrentUserId = _authenticationService.UserId;

		IsBusy = false;
        return Task.CompletedTask;
    }
}

