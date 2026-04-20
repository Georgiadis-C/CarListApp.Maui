using System;
using System.Collections.Generic;
using System.Text;
using CarListApp.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace CarListApp.Maui.ViewModels
{
    public partial class LogoutViewModel : BaseViewModel
    {
        public LogoutViewModel()
        {
            Logout();
        }

        [RelayCommand]
        async Task Logout()
        {
            SecureStorage.Remove("Token");
            App.UserInfo = null;
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}
