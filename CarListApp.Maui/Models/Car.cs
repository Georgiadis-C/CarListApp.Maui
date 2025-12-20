using System;
using System.Collections.Generic;
using System.Text;

namespace CarListApp.Maui.Models
{

    public class Car : BaseEntity
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }

    }
}
