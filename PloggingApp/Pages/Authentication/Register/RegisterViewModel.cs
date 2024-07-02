using CommunityToolkit.Mvvm.Input;
using PlogPal.Application;
using PlogPal.Application.Interfaces;
using PlogPal.Application.LoginManagement.Commands;
using PlogPal.Maui.Shared;

namespace PlogPal.Maui.Features.Authentication;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IEventBus _eventBus;
    private readonly IRegisterCommand _registerCommand;
    private readonly IToastService _toastService;
	public string Email { get; set; }
	public string Password { get; set; }
	public string DisplayName { get; set; }

	public RegisterViewModel(IRegisterCommand registerCommand, IToastService toastService)
	{
        _registerCommand = registerCommand;
        _toastService = toastService;
	}

	[RelayCommand]
	private async Task Register()
	{
		if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(DisplayName))
		{
			try
			{
				var result = await _registerCommand.RegisterUser(Email, Password, DisplayName);
				if (result.IsSuccess)
				{
                    await _toastService.MakeToast("Success. Account created.");
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
            catch (Exception)
            {
				await ShowErrorMessage();
			}
		}

		await ShowErrorMessage();
	}

	private Task ShowErrorMessage()
	{
        return _toastService.MakeToast("Could not register user");
	}
}