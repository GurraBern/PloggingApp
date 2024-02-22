using CommunityToolkit.Maui.Views;
using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.MVVM.Views;

public partial class AcceptPopup : Popup
{
    public AcceptPopup(AcceptPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}