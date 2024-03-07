using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class GenerateQRcodeViewModel : BaseViewModel
{
    public Task Initialization { get; private set; }

    public GenerateQRcodeViewModel()
	{
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

        //TODO replace with actual id when user authentication is implemented
        CurrentUserId = "444ajsldkfjasödjfk34";

		IsBusy = false;
        return Task.CompletedTask;
    }
}

