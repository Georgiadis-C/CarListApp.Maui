using CarListApp.Maui.ViewModels;

namespace CarListApp.Maui.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingViewModel loadingViewModel)
	{
		InitializeComponent();
        BindingContext = loadingViewModel;
    }
}