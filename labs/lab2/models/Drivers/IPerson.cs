using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.models.Drivers
{
    interface IPerson
    {
        uint Age { get; set; }
        string FullName { get; set; }
        bool IsAlive { get; }

        void Death();
    }
}
