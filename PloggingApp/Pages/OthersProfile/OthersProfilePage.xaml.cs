using Plogging.Core.Models;

namespace PloggingApp.Pages;
public partial class OthersProfilePage : ContentPage
{
    public OthersProfilePage(OthersProfilePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
