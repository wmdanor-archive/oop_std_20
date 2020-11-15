using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

using lab1.models.Drivers;
using lab1.models.Vehicles;
using lab1.models.DriverLicense;
using lab1.models.Other;
using lab1.models.UserDefinedExceptions;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Driver dr1 = new Driver("Omegalul Kek", 17);
            Driver dr2 = new ExperiencedDriver("Omegalul Sas", 32);
            Driver dr3 = new Driver("Omegalul Sos", 12);

            Console.WriteLine("exception test");
            try { Driver dr4 = new ExperiencedDriver("Omegalul Kok", 18); }
            catch (SmallAgeException e ) { Console.WriteLine("Low age excpetion caught: {0}, minimal age - {1}, entered age - {2}", e.Message, e.MinimalAge, e.InputAge); }


            DriverLicense lic1 = DriverLicense.CreateDriverLicense(dr1);
            DriverLicense lic2 = DriverLicense.CreateDriverLicense(dr2);
            DriverLicense lic3 = DriverLicense.CreateDriverLicense(dr3);
            if (lic3 == null) Console.WriteLine("lic3 is null");

            IPerson dr5 = new ExperiencedDriver("p", 35);
            IDrivingExperience dr6 = new ExperiencedDriver("r", 34);

            Console.WriteLine(dr5.OverrallExperience);
            Console.WriteLine(dr6.OverrallExperience);

            AVehicle veh1 = new Motorcicle("hf7834h87h32487hc8", Categories.A1);
            AVehicle veh2 = new Car("hjkfdjkadhkjf2", Categories.B, "Lul car", 0, 1596, 88, false);

            //VehicleCollection coll = new VehicleCollection();
            //coll.Add(veh1);

            dr1.AddVehicle(veh1);

            foreach (AVehicle veh in dr1.Vehicles)
            {
                Console.WriteLine(veh.VinCode);
            }

            Console.WriteLine("event, delegate test");
            Console.WriteLine("dr1 is_alive - {0}", dr1.IsAlive);
            Console.WriteLine("dr1 license is active - {0}", lic1.IsActive);
            Console.WriteLine("dr1 is driving - {0}", dr1.Drive("hf7834h87h32487hc8", 10));
            Console.WriteLine("dr1 is_alive - {0}", dr1.IsAlive);
            Console.WriteLine("dr1 license is active - {0}", lic1.IsActive);

            return;

            // serialization



            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("data.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, new Driver[] { dr1, dr2, (Driver)dr6 });
            }

            // десериализация из файла people.dat
            using (FileStream fs = new FileStream("data.dat", FileMode.OpenOrCreate))
            {
                Driver[] drs = (Driver[])formatter.Deserialize(fs);
            }

            using (FileStream fs = new FileStream("data_json.json", FileMode.OpenOrCreate))
            {
                string json = JsonConvert.SerializeObject(new Driver[] { dr1, dr2, (Driver)dr6 }, Formatting.Indented);
                byte[] info = new UTF8Encoding(true).GetBytes(json);
                fs.Write(info, 0, info.Length);
            }

            {
                string json = File.ReadAllText("data_json.json");
                Driver[] drs = JsonConvert.DeserializeObject<Driver[]>(json);
            }

            Console.WriteLine("\nPress any key");
            Console.ReadKey();
        }
    }
}
