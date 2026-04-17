using CarListApp.Maui.Services;
using CarListApp.Maui.ViewModels;
using CarListApp.Maui.Views;

namespace CarListApp.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        //Services
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "cars.db3");
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<CarDatabaseService>(s, dbPath));
        builder.Services.AddTransient<CarApiService>();

        //ViewModels
        builder.Services.AddSingleton<CarListViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LoadingViewModel>();
        builder.Services.AddTransient<CarDetailsViewModel>();

        //Views 
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<CarDetailsPage>();

        return builder.Build();
    }
}