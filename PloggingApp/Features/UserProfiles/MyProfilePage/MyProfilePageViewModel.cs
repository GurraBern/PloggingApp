namespace PloggingApp.Features.UserProfiles;

public class MyProfilePageViewModel
{
    public MyProfileViewModel MyProfileViewModel { get; set; }

    public MyProfilePageViewModel(MyProfileViewModel myProfileViewModel)
    {
        MyProfileViewModel = myProfileViewModel;
    }
}
