using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.ComponentModel;
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


            VehicleCollection vc1 = new VehicleCollection();
            Console.WriteLine("MaxGen {0}", GC.MaxGeneration);
            vc1.MakeGarbage();
            Console.WriteLine("Gen {0}", GC.GetGeneration(vc1));
            Console.WriteLine("Mem {0}", GC.GetTotalMemory(false));
            GC.Collect(0);
            Console.WriteLine("Gen {0}", GC.GetGeneration(vc1));
            Console.WriteLine("Mem {0}", GC.GetTotalMemory(false));
            GC.Collect(2);
            Console.WriteLine("Gen {0}", GC.GetGeneration(vc1));
            Console.WriteLine("Mem {0}", GC.GetTotalMemory(false));


            VehicleCollection vc2 = null;
            Console.WriteLine("\nNext");
            Console.WriteLine("Mem {0}", GC.GetTotalMemory(false));
            for (int j = 0; j < 10; j++)
            {
                vc2 = new VehicleCollection();
            }
            vc2 = null;
            Console.WriteLine("Mem {0}", GC.GetTotalMemory(false));
            GC.Collect();
            Console.WriteLine("Mem {0}", GC.GetTotalMemory(false));
            GC.WaitForPendingFinalizers();

            Console.WriteLine("");
            WeakReferenceTracker tracker = new WeakReferenceTracker(new Data(10), false);
            tracker.ReferenceDied +=
                () => Console.WriteLine("Reference is dead");

            GC.Collect(0);
            Thread.Sleep(100);

            GC.KeepAlive(tracker);

            Console.WriteLine("\nPress any key");
            Console.ReadKey();
        }

        class WeakReferenceTracker
        {
            private readonly WeakReference w;

            public WeakReferenceTracker(object o, bool trackResurection)
            {
                w = new WeakReference(o, trackResurection);

                Task.Factory.StartNew(TrackDeath);
            }

            public event Action ReferenceDied = () => { };

            private void TrackDeath()
            {
                while (true)
                {
                    if (!w.IsAlive)
                    {
                        ReferenceDied();
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
        }

        public class Data
        {
            private byte[] _data;
            private string _name;

            public Data(int size)
            {
                _data = new byte[size * 1024];
                _name = size.ToString();
            }

            public string Name
            {
                get { return _name; }
            }

            ~Data()
            {
                Console.WriteLine("Data destructor");
            }
        }
    }
}
