using System.Collections.ObjectModel;
using System.Diagnostics;
using CarListApp.Maui.Models;
using CarListApp.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarListApp.Maui.ViewModels
{
    public partial class CarListViewModel : BaseViewModel
    {
        const string editButtonText = "Update Car";
        const string createButtonText = "Add Car";

        public ObservableCollection<Car> Cars { get; private set; } = new();

        public CarListViewModel()
        {
            Title = "Car List";
            AddEditButtonText = createButtonText;
            GetCarList().Wait();
        }


        [ObservableProperty]
        bool isRefreshing;
        [ObservableProperty]
        string make;
        [ObservableProperty]
        string model;
        [ObservableProperty]
        string vin;
        [ObservableProperty]
        string addEditButtonText;
        [ObservableProperty]
        int carId;


        [RelayCommand]
        async Task GetCarList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Cars.Any()) Cars.Clear();

                var cars = App.CarService.GetCars();

                foreach (var car in cars)
                {
                    Cars.Add(car);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", "Failed to retrieve list of cars. ", "OK");
                Debug.WriteLine($"Unable to get car list: {ex.Message}");
                throw;

            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GetCarDetails(int id)
        {
            if (id == 0)
                return;

            await Shell.Current.GoToAsync($"{nameof(CarDetailsPage)}?Id={id}", true);
        }

        [RelayCommand]
        async Task SaveCar()
        {
            if (string.IsNullOrEmpty(Make) || string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Vin))
            {
                await Shell.Current.DisplayAlertAsync("Invalid data", "Please insert valid data.", "OK");
                return;
            }
            var car = new Car()
            {
                Make = Make,
                Model = Model,
                Vin = Vin
            }; 

            if (CarId != 0)
            {
                car.Id = CarId;
                App.CarService.UpdateCar(car);
                await Shell.Current.DisplayAlertAsync("Info", App.CarService.StatusMessage, "OK");
            }
            else
            {
                App.CarService.AddCar(car);
                await Shell.Current.DisplayAlertAsync("Info", App.CarService.StatusMessage, "OK");
            }

            await GetCarList();
            await ClearForm();

        }

        [RelayCommand]
        async Task DeleteCar(int id)
        {
            if (id == 0)
            {
                await Shell.Current.DisplayAlertAsync("Invalid Record", "Please try again.", "OK");
                return;
            }
            var result = App.CarService.DeleteCar(id);
            if (result == 0)
            {
                await Shell.Current.DisplayAlertAsync("Invalid Data", "Please insert vaild data.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Info", App.CarService.StatusMessage, "OK");
                await GetCarList();
            }
        }

        [RelayCommand]
        async Task UpdateCar(int id)
        {
            addEditButtonText = editButtonText;
        }

        [RelayCommand]
        async Task SetEditMode(int id)
        {
            AddEditButtonText = editButtonText;
            CarId = id;
            var car = App.CarService.GetCar(id);
            Make = car.Make;
            Model = car.Model;
            Vin = car.Vin;


        }


        [RelayCommand]
        public async Task ClearForm()
        {
            AddEditButtonText = createButtonText;
            CarId = 0;
            Make = string.Empty;
            Model = string.Empty;
            Vin = string.Empty;

            await Task.CompletedTask;
        }

    }
}