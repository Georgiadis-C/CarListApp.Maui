using System.Diagnostics;
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
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            CarService = carService;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            Debug.WriteLine("🔥 UNHANDLED EXCEPTION");
            Debug.WriteLine(ex?.Message);
            Debug.WriteLine(ex?.StackTrace);
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine("🔥 UNOBSERVED TASK EXCEPTION");
            Debug.WriteLine(e.Exception.Message);
            Debug.WriteLine(e.Exception.StackTrace);

            e.SetObserved(); // πολύ σημαντικό για να μην κλείσει η εφαρμογή
        }
    }
}