using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarListApp.Maui.Models;
using CarListApp.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarListApp.Maui.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(CarApiService carApiService)
        {
             this.carApiService = carApiService;
        }

        private readonly CarApiService carApiService;
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task Login()
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayLoginMessage("Invalid Login Attempt");
            }
            else
            {
                //Call API to attempt a login
                var loginModel = new LoginModel(username, password);

                var response = await carApiService.Login(loginModel);

                if (response == null)
                {
                    await DisplayLoginMessage(carApiService.StatusMessage);
                    return;
                }

                //Display message
                await DisplayLoginMessage(carApiService.StatusMessage);
                
                if (!string.IsNullOrEmpty(response.Token))
                {
                    //Store the token in secure storage
                    await SecureStorage.SetAsync("Token", response.Token);

                    //Build a menu on the fly based on the user role
                    var jsonToken = new JwtSecurityTokenHandler().ReadToken(response.Token) as JwtSecurityToken;

                    var role = jsonToken.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;

                    App.UserInfo = new UserInfo()
                    {
                        Username = Username,
                        Role = role
                    };

                    //Navigate to the app's main page
                    await Shell.Current.GoToAsync($"{nameof(MainPage)}");
                }
                else
                {
                    await DisplayLoginMessage("Invalid Login Attempt");
                }
            }
        }

         async Task DisplayLoginMessage(string message)
        {
            await Shell.Current.DisplayAlertAsync("Login Attempt Result", message, "OK");
            Password = string.Empty;
        }

    }
}
