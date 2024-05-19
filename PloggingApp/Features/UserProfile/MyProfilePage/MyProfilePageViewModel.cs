using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages;
public class MyProfilePageViewModel
{
    public MyProfileViewModel MyProfileViewModel { get; set; }
    public MyProfilePageViewModel(MyProfileViewModel myProfileViewModel)
    {
        MyProfileViewModel = myProfileViewModel;
    }
}
