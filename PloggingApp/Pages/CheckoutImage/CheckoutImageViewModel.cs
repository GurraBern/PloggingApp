using CommunityToolkit.Mvvm.Input;

namespace PloggingApp.Pages;

public partial class CheckoutImageViewModel
{
    public CheckoutImageViewModel()
    {

    }

    [RelayCommand]
    private static async Task UsePhoto()
    {
        //TODO upload image alongside the plogging session data
        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    [RelayCommand]
    private static async Task RetakePhoto()
    {
        await Shell.Current.GoToAsync("..");
    }
}
