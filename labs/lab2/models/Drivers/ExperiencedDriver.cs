using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;

using lab1.models.UserDefinedExceptions;

namespace lab1.models.Drivers
{
    class ExperiencedDriver : AmateurDriver
    {
        public override uint Age
        {
            get => base.Age;

            set
            {
                if (value >= 30) base.Age = value;
            }
        }

        public ExperiencedDriver(string full_name, uint age) : base(full_name, age)
        {
            if (age < 30) throw new SmallAgeException("Invalid age", 30, age);
            SortedDictionary<Categories, uint> temp = new SortedDictionary<Categories, uint>();
            temp.Add(Categories.A, 2);
            temp.Add(Categories.B, 6);
            temp.Add(Categories.C, 2);
            this.time_experience = temp;
        }

        public ExperiencedDriver(string full_name, uint age, SortedDictionary<Categories, uint> time_experience) : base(full_name, age)
        {
            uint result = 0;
            foreach (KeyValuePair<Categories, uint> entry in time_experience)
            {
                result += entry.Value;
            }
            if (result < 10) throw new SmallExperienceException("Invalid time experience", 10, result);
            this.time_experience = time_experience;
        }

        public override bool TimeExperienceUpdCheck(Categories category, uint experience)
        {
            uint exp;
            if (time_experience.TryGetValue(category, out exp))
            {
                if (exp > experience)
                {
                    uint overall = ((IDrivingExperience)this).OverrallExperience;
                    uint difference = exp - experience;
                    if (overall - difference < 10) return false;
                }
            }
            return true;
        }

        protected override double GetSkillFactor()
        {
            return (double)rand.Next(190, 210) / (double)100;
        }
    }
}
