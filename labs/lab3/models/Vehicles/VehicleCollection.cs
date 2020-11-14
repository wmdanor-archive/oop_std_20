using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lab1.models.Vehicles;

namespace lab1.models.Vehicles
{
    public class VehicleCollection : IEnumerable
    {
        private AVehicle[] vehicles;
        private int size;

        private const int default_capacity = 4;

        static readonly AVehicle[] empty_array = new AVehicle[0];

        public VehicleCollection()
        {
            vehicles = empty_array;
            size = 0;
        }


        public bool Contains(string vin_code)
        {
            foreach (AVehicle veh in vehicles)
            {
                if (veh.VinCode == vin_code)
                {
                    return true;
                }
            }
            return false;
        }
        
        public AVehicle Find(string vin_code)
        {
            try
            {
                return this[vin_code];
            }
            catch (ArgumentOutOfRangeException)
            {
                return default(AVehicle);
            }
        }

        public AVehicle this[string vin_code]
        {
            get
            {
                foreach (AVehicle veh in vehicles)
                {
                    if (veh.VinCode == vin_code)
                    {
                        return veh;
                    }
                }
                throw new ArgumentOutOfRangeException("Vin code", "Non-existent vehicle vin code in collection");
            }
            set
            {
                this.Add(value);
            }
        }

        public int Capacity
        {
            get
            {
                return vehicles.Length;
            }
            set
            {
                if (value < size)
                {
                    throw new ArgumentOutOfRangeException("Capacity", "Small capacity");
                }
                if (value != vehicles.Length)
                {
                    if (value > 0)
                    {
                        AVehicle[] newItems = new AVehicle[value];
                        if (size > 0)
                        {
                            Array.Copy(vehicles, 0, newItems, 0, size);
                        }
                        vehicles = newItems;
                    }
                    else
                    {
                        vehicles = empty_array;
                    }
                }
            }
        }

        public int Count { get => size; }

        public void Add(AVehicle vehicle)
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i].VinCode == vehicle.VinCode)
                {
                    vehicles[i] = vehicle;
                    return;
                }
            }
            if (size == vehicles.Length) EnsureCapacity(size + 1);
            vehicles[size++] = vehicle;
        }

        private void EnsureCapacity(int min)
        {
            if (vehicles.Length < min)
            {
                int new_capacity = vehicles.Length == 0 ? default_capacity : vehicles.Length * 2;
                if ((uint)new_capacity > 0X7FEFFFFF) new_capacity = 0X7FEFFFFF;
                if (new_capacity < min) new_capacity = min;
                Capacity = new_capacity;
            }
        }

        public bool Remove(string vin_code)
        {
            int index = IndexOf(vin_code);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public int IndexOf(string vin_code)
        {
            for (int i = 0; i < size; i++)
            {
                if (vehicles[i].VinCode == vin_code)
                {
                    return i;
                }
            }
            return -1;
        }

        private void RemoveAt(int index)
        {
            size--;
            Array.Copy(vehicles, index + 1, vehicles, index, size - index);
            vehicles[size] = default(AVehicle); ;
        }
        public void Clear()
        {
            if (size > 0)
            {
                Array.Clear(vehicles, 0, size);
                size = 0;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public VehicleNumerator GetEnumerator()
        {
            return new VehicleNumerator(this);
        }

        //

        public struct VehicleNumerator : IEnumerator
        {
            private VehicleCollection vehicles;
            private int position;
            private AVehicle current;
            internal VehicleNumerator(VehicleCollection vehicles)
            {
                this.vehicles = vehicles;
                position = 0;
                current = default(AVehicle); ;
            }

            object IEnumerator.Current {
                get 
                {
                    if (position == 0 || position == vehicles.size + 1)
                    {
                        throw new InvalidOperationException("Enumerator operation can't happen");
                    }
                    return Current;
                }
            }

            public AVehicle Current
            {
                get
                {
                    return current;
                }
            }

            public bool MoveNext()
            {
                if (position < vehicles.size)
                {
                    current = vehicles.vehicles[position];
                    position++;
                    return true;
                }
                else
                {
                    position = vehicles.size + 1;
                    current = default(AVehicle); ;
                    return false;
                }
            }

            public void Reset()
            {
                position = 0;
                current = default(AVehicle); ;
            }
        }
    }
}
