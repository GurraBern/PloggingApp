namespace PloggingApp.Pages;

public partial class MyProfilePage : ContentPage
{
	public MyProfilePage(MyProfilePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

}