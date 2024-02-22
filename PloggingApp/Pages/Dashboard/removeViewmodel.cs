using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages.Dashboard;

public partial class removeViewmodel
{
    private readonly IPopupService popupService;

    public removeViewmodel(IPopupService popupService)
    {
        this.popupService = popupService;
    }

    [RelayCommand]
    public async Task DisplayPopup()
    {
        await popupService.ShowPopupAsync<AcceptPopupViewModel>();
    }
}
