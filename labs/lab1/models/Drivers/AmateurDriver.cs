using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;

namespace lab1.models.Drivers
{
    class AmateurDriver : Driver
    {
        public override uint Age
        {
            get => base.Age;

            set
            {
                if (value >= 21) base.Age = value;
            }
        }

        public AmateurDriver(string full_name, uint age) : base(full_name, age)
        {
            if (age < 21) throw new ArgumentException("Invalid age (<21)");
            SortedDictionary<Categories, uint> temp = new SortedDictionary<Categories, uint>();
            temp.Add(Categories.A, 2);
            temp.Add(Categories.B, 2);
            temp.Add(Categories.C, 1);
            this.time_experience = temp;
        }

        public AmateurDriver(string full_name, uint age, SortedDictionary<Categories, uint> time_experience) : base(full_name, age)
        {
            uint result = 0;
            foreach (KeyValuePair<Categories, uint> entry in time_experience)
            {
                result += entry.Value;
            }
            if (result < 5) throw new ArgumentException("Invalid time experience");
            this.time_experience = time_experience;
        }

        public override bool TimeExperienceUpdCheck(Categories category, uint experience)
        {
            uint exp;
            if (time_experience.TryGetValue(category, out exp))
            {
                if (exp > experience)
                {
                    uint overall = this.OverrallExperience;
                    uint difference = exp - experience;
                    if (overall - difference < 5) return false;
                }
            }
            return true;
        }

        protected override double GetSkillFactor()
        {
            return (double)rand.Next(140, 160) / (double)100;
        }
    }
}
