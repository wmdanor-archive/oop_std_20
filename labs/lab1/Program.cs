using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Drivers;
using lab1.models.Vehicles;
using lab1.models.DriverLicense;
using lab1.models.Other;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Driver dr1 = new Driver("Omegalul Kek", 17);
            Driver dr2 = new ExperiencedDriver("Omegalul Sas", 32);
            Driver dr3 = new Driver("Omegalul Sos", 12);
            DriverLicense lic1 = DriverLicense.CreateDriverLicense(dr1);
            DriverLicense lic2 = DriverLicense.CreateDriverLicense(dr2);
            DriverLicense lic3 = DriverLicense.CreateDriverLicense(dr3);
            if (lic3 == null) Console.WriteLine("lic3 is null");

            AVehicle veh1 = new Motorcicle("hf7834h87h32487hc8", Categories.A1);
            AVehicle veh2 = new Car("hjkfdjkadhkjf2", Categories.B, "Lul car", 0, 1596, 88, false);

            Console.WriteLine("\nPress any key");
            Console.ReadKey();
        }
    }
}
