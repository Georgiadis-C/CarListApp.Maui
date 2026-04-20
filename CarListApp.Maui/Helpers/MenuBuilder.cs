using System;
using System.Collections.Generic;
using System.Text;
using CarListApp.Maui.Controls;

namespace CarListApp.Maui.Helpers
{
    public static class MenuBuilder
    {
        public  static void BuildMenu()
        {
            Shell.Current.Items.Clear();

            Shell.Current.FlyoutHeader = new FlyOutHeader();

            var role = App.UserInfo.Role;

            if (role == ("Administrator"))
            {
                var flyOutItem = new FlyoutItem()
                {
                    Title = "Admin Car Management",
                    Route = nameof(MainPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = "dotnet_bot.png",
                            Title = "Admin Page 1",
                            ContentTemplate = new DataTemplate(typeof(MainPage)),
                            Route = "Admin1"
                        },
                        new ShellContent
                        {
                            Icon = "dotnet_bot.png",
                            Title = "Admin Page 2",
                            ContentTemplate = new DataTemplate(typeof(MainPage)),
                            Route = "Admin2"
                        }
                    }
                };

                if (!Shell.Current.Items.Contains(flyOutItem))
                {
                    Shell.Current.Items.Add(flyOutItem);
                }
            }
            if (role == ("User"))
            {
                var flyOutItem = new FlyoutItem()
                {
                    Title = "USer Car Management",
                    Route = nameof(MainPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = "dotnet_bot.png",
                            Title = "User Page 1",
                            ContentTemplate = new DataTemplate(typeof(MainPage)),
                            Route = "User1"
                        },
                        new ShellContent
                        {
                            Icon = "dotnet_bot.png",
                            Title = "User Page 2",
                            ContentTemplate = new DataTemplate(typeof(MainPage)),
                            Route = "User2"
                        }
                    }
                };
                if (!Shell.Current.Items.Contains(flyOutItem))
                {
                    Shell.Current.Items.Add(flyOutItem);
                }
            }
        }


    }
}
