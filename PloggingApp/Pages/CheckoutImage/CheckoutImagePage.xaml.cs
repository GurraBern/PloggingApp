namespace PloggingApp.Pages;

public partial class CheckoutImagePage : ContentPage
{
    public CheckoutImagePage(CheckoutImageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}