using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace PloggingApp.MVVM.Views;

public partial class LitterTypeButton : ContentView
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(LitterTypeButton), null);

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(LitterTypeButton), null);

    public object CommandParameter
    {
        get { return (object)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(LitterTypeButton), default(string));

    public string ImageSource
    {
        get { return (string)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    private int counter = 0; 
	public LitterTypeButton()
	{
		InitializeComponent();
	}

    private async void IncrementCounter(object sender, TappedEventArgs e)
    {
		counter++;
		counterLabel.Text = counter.ToString();
        await this.ScaleTo(0.9, 30, Easing.CubicInOut);
        await this.ScaleTo(1, 30, Easing.CubicInOut);
    }
}