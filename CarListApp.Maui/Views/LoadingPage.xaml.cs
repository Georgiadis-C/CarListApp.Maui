using CarListApp.Maui.ViewModels;

namespace CarListApp.Maui.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Καλούμε τη μέθοδο αφού η σελίδα έχει εμφανιστεί
        if (BindingContext is LoadingViewModel vm)
        {
            await vm.CheckUserLoginDetails();
        }
    }
} 
