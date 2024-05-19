using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Pages;

namespace PloggingApp.MVVM.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class GenerateQRcodeViewModel : ObservableObject 
{
	[ObservableProperty]
	private string userId;

	[RelayCommand]
	private static async Task GoBack() => await Shell.Current.GoToAsync(nameof(PlogTogetherPage));
}

