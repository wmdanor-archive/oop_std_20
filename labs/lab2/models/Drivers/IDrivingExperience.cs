using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;

namespace lab1.models.Drivers
{
    interface IDrivingExperience
    {
        uint GetCategoryTimeExperience(Categories category);
        SortedDictionary<Categories, uint> TimeExperience { get; }
        bool TimeExperienceUpdCheck(Categories category, uint experience);
        uint OverrallExperience { get; }
    }
}
