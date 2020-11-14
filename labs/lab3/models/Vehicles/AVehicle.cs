using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;

namespace lab1.models.Vehicles
{
    public abstract class AVehicle : IComparable<AVehicle>
    {
        public int CompareTo(AVehicle other)
        {
            return string.Compare(this.VinCode, other.VinCode);
        }

        public AVehicle(string vin_code, Categories category, string name, uint mileage_km, uint engine_displacement, uint engine_power_hp, bool is_broken) : this(vin_code, category)
        {
            Name = name;
            MileageKm = mileage_km;
            EngineDisplacement = engine_displacement;
            EnginePowerHP = engine_power_hp;
            this.is_broken = is_broken;
        }

        public AVehicle(string vin_code, Categories category)
        {
            this.vin_code = vin_code;
            this.Category = category;
        }

        private readonly string vin_code;

        public string Name { get; set; }
        
        public Categories Category { get; set; }

        public string VinCode { get => vin_code; }

        private bool is_broken = false;

        public bool IsBroken { get => is_broken; }

        public uint MileageKm { get; set; }

        public uint EngineDisplacement{ get; set; }

        public uint EnginePowerHP { get; set; }

        public void BreakVehicle()
        {
            is_broken = true;
        }

        public void RepairVehicle()
        {
            is_broken = false;
        }

        public abstract bool Ride(uint distance);
    }
}
