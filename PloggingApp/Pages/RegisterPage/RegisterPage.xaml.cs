using PloggingApp.MVVM.ViewModels;

namespace PloggingApp.Pages


{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new AuthenticationViewModel(Navigation);
        }
    }
}

