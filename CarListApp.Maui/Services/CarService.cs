using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CarListApp.Maui.Models;

namespace CarListApp.Maui.Services
{
    public class CarService
    {
        public List<Car> GetCars() {
            return new List<Car>
            {
                new Car { Id = 1, Make = "Toyota", Model = "Camry", Vin = "1HGCM82633A123456" },
                new Car { Id = 2, Make = "Honda", Model = "Accord", Vin = "1HGCM82633A654321" },
                new Car { Id = 3, Make = "Ford", Model = "Mustang", Vin = "1FAFP404X1F123456" },
                new Car { Id = 4, Make = "Chevrolet", Model = "Malibu", Vin = "1G1ZD5ST0JF123456" },
                new Car { Id = 5, Make = "Nissan", Model = "Altima", Vin = "1N4AL3AP7JC123456" }

            };


        }
    }
}
