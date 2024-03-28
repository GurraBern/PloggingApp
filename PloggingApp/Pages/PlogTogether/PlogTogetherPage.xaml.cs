using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class PlogTogetherPage : ContentPage
{
	public PlogTogetherPage(PlogTogetherViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
	}
}
