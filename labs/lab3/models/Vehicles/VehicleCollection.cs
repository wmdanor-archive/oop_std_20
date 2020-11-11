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
    class VehicleCollection : IEnumerable
    {
        private AVehicle[] vehicles;
        private int size;

        private const int default_capacity = 4;

        static readonly AVehicle[] empty_array = new AVehicle[0];


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
                return null;
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
                for (int i = 0; i < vehicles.Length; i++)
                {
                    if (vehicles[i].VinCode == value.VinCode)
                    {
                        vehicles[i] = value;
                        return;
                    }
                }
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

        public int Count { get => vehicles.Length; }

        public void Add(AVehicle vehicle)
        {
            if (size == vehicles.Length) EnsureCapacity(size + 1);  // vin code copy check
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
            vehicles[size] = null;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public VehicleNumerator GetEnumerator()
        {
            return new VehicleNumerator(vehicles);
        }
    }

    class VehicleNumerator : IEnumerator
    {
        private AVehicle[] vehicles;
        private int position = -1;
        public VehicleNumerator(AVehicle[] vehicles)
        {
            this.vehicles = vehicles;
        }

        object IEnumerator.Current { get => this.Current; }

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
