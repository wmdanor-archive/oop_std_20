using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;

namespace lab1.models.Drivers
{
    public interface IDrivingExperience
    {
        bool SetCategoryTimeExperience(Categories category, uint experience);
        bool TimeExperienceUpdCheck(Categories category, uint experience);
        uint OverrallExperience { get; }
        uint GetCategoryTimeExperience(Categories category);
        SortedDictionary<Categories, uint> TimeExperience { get; }
    }
}
