using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using lab1.models.Vehicles;
using Newtonsoft.Json;

namespace lab1.models.Vehicles
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class VehicleCollection : ICollection, IEnumerable, IDisposable
    {
        [JsonProperty]
        private AVehicle[] vehicles;
        [JsonProperty]
        private int size;
        [NonSerialized]
        private bool disposed = false;
        [NonSerialized]
        private Component component = new Component();

        private const int default_capacity = 4;

        static readonly AVehicle[] empty_array = new AVehicle[0];

        [JsonProperty]
        private Object sync_root;

        public VehicleCollection()
        {
            vehicles = empty_array;
            size = 0;
        }

        public void Dispose()
        {
            Console.WriteLine("45678");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    component.Dispose();
                    Console.WriteLine("rutyryty");
                }
                disposed = true;
            }
        }

        ~VehicleCollection()
        {
            Dispose(false);
        }

        public void MakeGarbage()
        {
            Component c;
            for (int i = 0; i < 1024; i++) c = new Component();
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

        public bool IsSynchronized { get => false; }

        public object SyncRoot
        {
            get
            {
                if (sync_root == null)
                {
                    System.Threading.Interlocked.CompareExchange<Object>(ref sync_root, new Object(), null);
                }
                return sync_root;
            }
        }

        public void CopyTo(Array array, int index)
        {
            if ((array != null) && (array.Rank != 1))
            {
                throw new ArgumentException("Multidimnesional arrays are not supported");
            }

            try
            {
                Array.Copy(vehicles, 0, array, index, size);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type");
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
