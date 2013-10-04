using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNMC2013.JSON.Activities
{
    public class D
    {
        public List<Activity> results { get; set; }
    }

    public class RootObject
    {
        public D d { get; set; }
    }
}
