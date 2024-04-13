namespace PloggingApp.Pages;

public partial class CreateEventDetails : ContentPage
{
    public CreateEventDetails(CreateEventDetailsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}