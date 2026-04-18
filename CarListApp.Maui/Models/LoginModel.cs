using System;
using System.Collections.Generic;
using System.Text;

namespace CarListApp.Maui.Models
{
    public class LoginModel
    {
        public LoginModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
