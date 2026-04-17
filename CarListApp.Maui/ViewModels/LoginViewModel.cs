using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarListApp.Maui.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task Login()
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayLoginError();
            }
            else
            {
                //Call API to attempt a login
                var loginSuccessful = true;

                if(loginSuccessful)
                {
                    //Display welcome message

                    //Build a meny on the fly based on the user role

                    //Navigate to the app's main page
                }

                await DisplayLoginError();
            }
        }

         async Task DisplayLoginError()
        {
            await Shell.Current.DisplayAlertAsync("Invalid Attempt", "Invalid Username or Password", "OK");
            Password = string.Empty;
        }

    }
}
