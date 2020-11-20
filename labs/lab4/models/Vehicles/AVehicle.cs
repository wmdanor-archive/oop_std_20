using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using lab1.models.Other;

namespace lab1.models.Vehicles
{
    public class AVehicleSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(AVehicle).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    public class AVehicleConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new AVehicleSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(AVehicle));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            JObject jo = JObject.Load(reader);
            switch (jo["VehicleType"].Value<int>())
            {
                case 1:
                    return JsonConvert.DeserializeObject<Motorcicle>(jo.ToString(), SpecifiedSubclassConversion);
                case 2:
                    return JsonConvert.DeserializeObject<Car>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }

    [JsonConverter(typeof(AVehicleConverter))]
    [Serializable]
    public abstract class AVehicle : IComparable<AVehicle>
    {

        private readonly string vin_code;

        public int CompareTo(AVehicle other)
        {
            return string.Compare(this.VinCode, other.VinCode);
        }

        protected AVehicle() { }
        
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

        [JsonProperty]
        protected int VehicleType { get; set; }

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
