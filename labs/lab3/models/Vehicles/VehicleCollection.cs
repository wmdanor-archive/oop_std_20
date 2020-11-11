using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab1.models.Vehicles;

namespace lab1.models.Vehicles
{
    class VehicleCollection : INumerable
    {
        private AVehicle[] vehicles;
        public VehicleCollection(AVehicle[] vehicles)
        {
            this.vehicles = vehicles;
        }
    }

    class VehicleNumerator : INumerator
    {
        private AVehicle[] vehicles;
        private int position = -1;
        public VehicleNumerator(AVehicle[] vehicles)
        {
            this.vehicles = vehicles;
        }

        object INumerator.Current { get => this.Current; }

        public AVehicle Current
        {
            get
            {
                try
                {
                    return vehicles[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public bool MoveNext()
        {
            if (position < vehicles.Length - 1)
            {
                position++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
