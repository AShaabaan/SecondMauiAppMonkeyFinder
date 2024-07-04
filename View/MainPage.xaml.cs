using SecondMauiAppMonkeyFinder.ViewModels;

namespace SecondMauiAppMonkeyFinder.View;

public partial class MainPage : ContentPage
{
	public MainPage(MonkeyViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}