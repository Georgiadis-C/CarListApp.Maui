using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CarListApp.Maui.Models;
using SQLite;

namespace CarListApp.Maui.Services
{
    public class CarService
    {
        SQLiteConnection conn;
        string _dbPath;
        public string StatusMessage;

        public CarService(string dbPath)
        {
            _dbPath = dbPath;
        }

        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Car>();

        }
        public List<Car> GetCars() {
            try
            {
                Init();
                return conn.Table<Car>().ToList();
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }

            return new List<Car>();


        }
    }
}
