using CommunityToolkit.Mvvm.ComponentModel;

namespace PloggingApp.Features.Plogtogether;

public partial class PlogUser : ObservableObject
{
	[ObservableProperty]
	private string displayName;

    [ObservableProperty]
    private string userId;

    [ObservableProperty]
    private bool showButtons;
}

