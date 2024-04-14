namespace PloggingApp.Pages;

public partial class MyProfilePage : ContentPage
{
    private readonly MyProfilePageViewModel vm;
   public MyProfilePage(MyProfilePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await vm.MyProfileViewModel.GetSessions();
    }

}