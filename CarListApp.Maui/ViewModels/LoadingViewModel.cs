using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CarListApp.Maui.Views;

namespace CarListApp.Maui.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {


        public async Task CheckUserLoginDetails()
        {
            //Retrieve token from internal storage
            await Task.Delay(500);

            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(token))
            {
               await GoToLoginPage();
            }
            else
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                
                if (jsonToken.ValidTo <DateTime.UtcNow)
                {
                    SecureStorage.Remove("Token");
                    await GoToLoginPage();
                }
                else
                {
                    //Token is valid, go to main page
                    await GoToMainPage();
                }
            }

            // Evaluate token and decide if valid

        }

        private async Task GoToLoginPage()
        {
            // Το // είναι απαραίτητο για absolute routing
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        private async Task GoToMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
