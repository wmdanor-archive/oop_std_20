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

        public delegate void DriverDeathDelegate();
        public event DriverDeathDelegate DriverDeathEvent;

        private uint age;
        private bool is_alive = true;

        protected SortedDictionary<Categories, uint> time_experience;

        private VehicleCollection vehicles;


        #region Constructors 

        static Driver()
        {
            rand = new Random();
            general_luck = rand.NextDouble();
        }

        public Driver(string full_name, uint age, SortedDictionary<Categories, uint> time_experience) : this(full_name, age)
        {
            this.time_experience = time_experience;
        }

        public Driver(string full_name, uint age) : this()
        {
            this.Name = full_name;
            this.age = age;
        }
        
        protected Driver()
        {
            drivers_amount++;
            //time_experience = new SortedDictionary<Categories, uint>();
            //vehicles = new SortedDictionary<string, AVehicle>();
            DriverDeathEvent += Death;
        }

        #endregion

        ~Driver()
        {
            drivers_amount--;
        }


        public virtual uint Age
        {
            get => age;
            set
            {
                age = value;
            }
        }

        public string Name { get; set; }

        uint IPerson.OverrallExperience { get; set; }


        #region Driving

        public DrivingResult Drive(string vin_code, uint distance)
        {
            if (IsAlive == false) return DrivingResult.AlreadyDead;
            AVehicle veh = vehicles.Find(vin_code);
            if (veh == null) return DrivingResult.NoVehicle;
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

        private double GetDrivinngProcessFactor()   // стаж, кол-во водителей, тип водителя, удача, умение водителя
        {
            double factor = 1 / (2 * (add_factor + general_luck) * GetSkillFactor());
            return factor;
        }

        protected virtual double GetSkillFactor()
        {
            return (double)rand.Next(90, 110) / (double)100;
        }

        #endregion


        public void Death()
        {
            is_alive = false;
        }

        public bool IsAlive { get => is_alive; }


        #region Driving experience

        public bool SetCategoryTimeExperience(Categories category, uint experience)
        {
            if (!TimeExperienceUpdCheck(category, experience))
            {
                return false;
            }
            if (experience == 0)
            {
                time_experience.Remove(category);
            }
            else
            {
                time_experience[category] = experience;
            }
            return true;
        }

        public virtual bool TimeExperienceUpdCheck(Categories category, uint experience)
        {
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

        public uint GetCategoryTimeExperience(Categories category)
        {
            uint exp = 0;
            time_experience.TryGetValue(category, out exp);
            return exp;
        }

        public SortedDictionary<Categories, uint> TimeExperience { get => time_experience; }

        #endregion


        #region Vehicles

        public void AddVehicle(AVehicle vehicle)
        {
            vehicles[vehicle.VinCode] = vehicle;
        }

        public AVehicle RemoveVehicle(string vin_code)
        {
            AVehicle veh = vehicles.Find(vin_code);
            vehicles.Remove(vin_code);
            return veh;
        }

        public VehicleCollection Vehicles { get => vehicles; }

        #endregion
    }
}
