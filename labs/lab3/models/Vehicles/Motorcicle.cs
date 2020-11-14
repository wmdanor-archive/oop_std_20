﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;

namespace lab1.models.Vehicles
{
    public class Motorcicle : AVehicle
    {
        public Motorcicle(string vin_code, Categories category, string name, uint mileage_km, uint engine_displacement, uint engine_power_hp, bool is_broken) :
            base(vin_code, category, name, mileage_km, engine_displacement, engine_power_hp, is_broken)
        { }

        public Motorcicle(string vin_code, Categories category) : base(vin_code, category) { }
        

        public override bool Ride(uint distance)
        {
            MileageKm += distance;
            double factor = 1 - Math.Exp(-(double)MileageKm * 2.5 * Math.Pow(10, -5));
            Random rand = new Random();
            if (rand.NextDouble() < factor)
            {
                BreakVehicle();
                return false;
            }
            return true;
        }
    }
}
