using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarListApp.Maui.Models;
using CarListApp.Maui.Services;
using CarListApp.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarListApp.Maui.ViewModels
{
    public partial class CarListViewModel : BaseViewModel
    {
        private readonly Services.CarService carService;

        public ObservableCollection<Car> Cars { get; private set; } = new();

        public CarListViewModel(Services.CarService carService)
        {
            Title = "Car List";
            this.carService = carService;
        }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetCarList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Cars.Any()) Cars.Clear();

                var cars = carService.GetCars();
                foreach (var car in cars)
                {
                    Cars.Add(car);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get car list: {ex.Message}");
                throw;
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve list of cars. ", "OK");

            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GetCarDetails(Car car)
        {
            if (car == null)
                return;

            await Shell.Current.GoToAsync(nameof(CarDetailsPage), true, new Dictionary <string, object>
            {
                {nameof(Car), car },
            });
        }
    }
}
