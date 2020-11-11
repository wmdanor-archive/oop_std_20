using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.models.UserDefinedExceptions
{
    class SmallAgeException : Exception
    {
        public uint MinimalAge { get; }
        public uint InputAge { get; }
        
        public SmallAgeException(string message, uint minimal_age, uint input_age) : base(message)
        {
            MinimalAge = minimal_age;
            InputAge = input_age;
        }
    }
}
