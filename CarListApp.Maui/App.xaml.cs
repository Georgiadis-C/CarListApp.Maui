using CarListApp.Maui.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarListApp.Maui
{
    public partial class App : Application
    {
        public static CarService CarService { get; private set; }  
        public App(CarService carService)
        {
            InitializeComponent();

            MainPage = new AppShell();
            CarService = CarService;
        }
    }
}