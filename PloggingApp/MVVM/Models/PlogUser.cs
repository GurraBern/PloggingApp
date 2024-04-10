using CommunityToolkit.Mvvm.ComponentModel;


namespace PloggingApp.MVVM.Models;

public partial class PlogUser : ObservableObject
{
	[ObservableProperty]
	public string displayName;
}

