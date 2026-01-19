using System;
using System.Collections.Generic;
using System.Text;
using CarListApp.Maui.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarListApp.Maui.ViewModels
{
    [QueryProperty(nameof(Car), "Car")]
    public partial class CarDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Car car;

    }
}
