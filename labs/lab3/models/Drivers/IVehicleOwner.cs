using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Vehicles;

namespace lab1.models.Drivers
{
    interface IVehicleOwner
    {     
        void AddVehicle(AVehicle vehicle);

        AVehicle RemoveVehicle(string vin_code);

        SortedDictionary<string, AVehicle> Vehicles { get; }
    }
}
