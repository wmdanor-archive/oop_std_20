using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;
using lab1.models.Vehicles;

namespace lab1.models.Drivers
{
    class Driver : IPerson, IVehicleOwner, IDrivingExperience
    {
        private readonly static double general_luck;
        protected static uint drivers_amount;
        protected static Random rand;
        private const double crash_factor = 0.00548;
        private const double death_factor = 28.3;
        private const double add_factor = 0.20889;


        private uint age;
        public virtual uint Age
        {
            get => age;
            set
            {
                age = value;
            }
        }

        public string Name { get; set; }

        private bool is_alive;
        public bool IsAlive { get => is_alive; }

        public void Death()
        {
            is_alive = false;
        }

        //public event void DriverDeathEvent;
        public delegate void DriverDeathDelegate();
        public event DriverDeathDelegate DriverDeathEvent;

        static Driver()
        {
            rand = new Random();
            general_luck = rand.NextDouble();
        }

        protected Driver()
        {
            drivers_amount++;
            is_alive = true;
            time_experience = new SortedDictionary<Categories, uint>();
            vehicles = new SortedDictionary<string, AVehicle>();
            DriverDeathEvent += Death;
        }

        ~Driver()
        {
            drivers_amount--;
        }

        public Driver(string full_name, uint age) : this()
        {
            this.Name = full_name;
            this.age = age;
        }

        public Driver(string full_name, uint age, SortedDictionary<Categories, uint> time_experience) : this(full_name, age)
        {
            this.time_experience = time_experience;
        }

        protected SortedDictionary<Categories, uint> time_experience;

        public uint GetCategoryTimeExperience(Categories category)
        {
            uint exp = 0;
            time_experience.TryGetValue(category, out exp);
            return exp;
        }

        public SortedDictionary<Categories, uint> TimeExperience { get => time_experience; }

        public virtual bool TimeExperienceUpdCheck(Categories category, uint experience)
        {
            return true;
        }

        public bool SetCategoryTimeExperience(Categories category, uint experience)
        {
            if (!TimeExperienceUpdCheck(category, experience)) return false;
            if (experience == 0) time_experience.Remove(category);
            else time_experience[category] = experience;
            return true;
        }

        uint IDrivingExperience.OverrallExperience
        {
            get
            {
                uint result = 0;
                foreach (KeyValuePair<Categories, uint> entry in time_experience)
                {
                    result += entry.Value;
                }
                return result;
            }
        }

        uint IPerson.OverrallExperience { get; set; }


        private SortedDictionary<string, AVehicle> vehicles;

        public SortedDictionary<string, AVehicle> Vehicles { get => vehicles; }

        public void AddVehicle(AVehicle vehicle)
        {
            if (vehicles.ContainsKey(vehicle.VinCode)) return;
            else vehicles[vehicle.VinCode] = vehicle;
        }

        public AVehicle RemoveVehicle(string vin_code)
        {
            AVehicle veh = null;
            vehicles.TryGetValue(vin_code, out veh);
            vehicles.Remove(vin_code);
            return veh;
        }

        protected virtual double GetSkillFactor()
        {
            return (double)rand.Next(90, 110) / (double)100;
        }

        private double GetDrivinngProcessFactor()   // стаж, кол-во водителей, тип водителя, удача, умение водителя
        {
            double factor = 1 / (2 * (add_factor + general_luck) * GetSkillFactor());
            return factor;
        }

        private bool DrivingProcessResult()
        {
            double factor = GetDrivinngProcessFactor() * crash_factor;
            double fate = rand.NextDouble() * 100;
            bool result = fate <= factor;
            //return true;    // подкрутка для демонстрации
            return result;
        }

        private bool DeathInAccident()
        {
            double factor = GetDrivinngProcessFactor() * death_factor;
            double fate = rand.NextDouble() * 100;
            bool result = fate <= factor;
            //return true;    // подкрутка для демонстрации
            return result;
        }

        public DrivingResult Drive(string vin_code, uint distance)
        {
            if (IsAlive == false) return DrivingResult.AlreadyDead;
            AVehicle veh = null;
            if (!vehicles.TryGetValue(vin_code, out veh)) return DrivingResult.NoVehicle;
            if (!veh.Ride(distance)) return DrivingResult.VehicleBroken;
            if (DrivingProcessResult())
            {
                veh.BreakVehicle();
                if (DeathInAccident())
                {
                    DriverDeathEvent?.Invoke();
                    return DrivingResult.Death;
                }
                else
                {
                    return DrivingResult.Crash;
                }
            }
            else return DrivingResult.Success;
        }
    }
}
