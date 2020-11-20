using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.models.UserDefinedExceptions
{
    public class SmallExperienceException : Exception
    {
        public uint MinimalExperience { get; }
        public uint InputExperience { get; }

        public SmallExperienceException(string message, uint minimal_exprerience, uint input_exprerience) : base(message)
        {
            MinimalExperience = minimal_exprerience;
            InputExperience = input_exprerience;
        }
    }
}
