using System;
using System.Collections.Generic;
using System.Text;
using CarListApp.Maui.Views;

namespace CarListApp.Maui.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {

        public LoadingViewModel()
        {
            CheckUserLoginDetails();
        }

        private async void CheckUserLoginDetails()
        {
            //Retrieve token from internal storage

            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(token))
            {
               await GoToLoginPage();
            }

            // Evaluate token and decide if valid

        }

        private async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        private async Task GoToMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
