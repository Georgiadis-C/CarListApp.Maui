using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarListApp.Maui.Helpers;
using CarListApp.Maui.Models;
using CarListApp.Maui.Views;
using MenuBuilder = CarListApp.Maui.Helpers.MenuBuilder;

namespace CarListApp.Maui.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {
        public async Task CheckUserLoginDetails()
        {
            await Task.Delay(500);

            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrWhiteSpace(token))
            {
                await GoToLoginPage();
                return;
            }

            var handler = new JwtSecurityTokenHandler();


            if (!handler.CanReadToken(token))
            {
                SecureStorage.Remove("Token");
                await GoToLoginPage();
                return;
            }

            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;


            if (jsonToken == null)
            {
                SecureStorage.Remove("Token");
                await GoToLoginPage();
                return;
            }


            if (jsonToken.ValidTo < DateTime.UtcNow)
            {
                SecureStorage.Remove("Token");
                await GoToLoginPage();
                return;
            }


            var role = jsonToken.Claims.FirstOrDefault(c =>c.Type == ClaimTypes.Role ||c.Type == "role" || c.Type.EndsWith("/role"))?.Value ?? "User";

            var email = jsonToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value?.ToLower();

            App.UserInfo = new UserInfo()
            {
                Username = email,
                Role = role
            };

            MenuBuilder.BuildMenu();
            await GoToMainPage();
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