using PloggingApp.MVVM.Models;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;

public partial class MyProfilePage : ContentPage
{
	public MyProfilePage(MyProfileViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

}