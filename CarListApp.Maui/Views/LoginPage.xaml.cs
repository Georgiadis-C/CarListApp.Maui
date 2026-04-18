using CarListApp.Maui.ViewModels;

namespace CarListApp.Maui.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
        BindingContext = loginViewModel;

    }


}