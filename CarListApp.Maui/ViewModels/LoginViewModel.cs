using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarListApp.Maui.Helpers;
using CarListApp.Maui.Models;
using CarListApp.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarListApp.Maui.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly CarApiService carApiService;

        public LoginViewModel(CarApiService carApiService)
        {
            this.carApiService = carApiService;
        }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await DisplayLoginMessage("Invalid Login Attempt");
                return;
            }

            var loginModel = new LoginModel(Username, Password);
            var response = await carApiService.Login(loginModel);

            if (response == null)
            {
                await DisplayLoginMessage(carApiService.StatusMessage);
                return;
            }

            await DisplayLoginMessage(carApiService.StatusMessage);


            if (string.IsNullOrWhiteSpace(response.Token))
            {
                await DisplayLoginMessage("Invalid Login Attempt");
                return;
            }

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(response.Token))
            {
                await DisplayLoginMessage("Invalid Token");
                return;
            }

            var jsonToken = handler.ReadToken(response.Token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                await DisplayLoginMessage("Token parsing failed");
                return;
            }


            var role = jsonToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var email = jsonToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;


            await SecureStorage.SetAsync("Token", response.Token);


            App.UserInfo = new UserInfo()
            {
                Username = email ?? Username,
                Role = role ?? "User"
            };
            Debug.WriteLine($"ROLE FROM LOGIN: {role}");

            MenuBuilder.BuildMenu();
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }

        async Task DisplayLoginMessage(string message)
        {
            await Shell.Current.DisplayAlertAsync("Login Attempt Result", message, "OK");
            Password = string.Empty;
        }
    }
}