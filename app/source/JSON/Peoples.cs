using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNMC2013.JSON.Peoples
{
    public class D
    {
        public List<Person> results { get; set; }
    }

    public class RootObject
    {
        public D d { get; set; }
    }
}
